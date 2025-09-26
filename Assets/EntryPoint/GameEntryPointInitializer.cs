using System;
using System.Collections;
using System.Collections.Generic;
using Common.CoroutinePerformer;
using Common.Disposables;
using Common.LoadingCurtain;
using Common.Mediators;
using Common.Sounds.Scripts;
using EntryPoint.Mediators;
using Infrastructure.Bootstraps;
using Infrastructure.DI;
using Services.Integrations;
using Services.PlayerData;
using Services.PlayerData.Core;
using Services.PlayerData.Core.Language;
using Services.PlayerData.Core.Levels;
using Services.PlayerData.Core.Wallet;
using Services.PlayerData.SavingStrategy;
using Services.PurchaseService;
using Services.SceneManagement;
using UnityEngine;

namespace EntryPoint
{
    public class GameEntryPointInitializer: IBootstrapInitializer
    {
        public IEnumerator Initialize(IDIContainer container)
        {
            ILoadingCurtainService loadingCurtainService = container.Get<ILoadingCurtainService>();
            loadingCurtainService.Show();
            
            SaveLoadService saveLoadService = container.Get<SaveLoadService>();
            WalletService walletService = container.Get<WalletService>();
            LevelsService levelsService = container.Get<LevelsService>();
            LanguageService languageService = container.Get<LanguageService>();
            DataChangedSavingStrategy savingStrategy = container.Get<DataChangedSavingStrategy>();
            
            saveLoadService.Register(walletService, savingStrategy);
            saveLoadService.Register(levelsService, savingStrategy);
            saveLoadService.Register(languageService, savingStrategy);

            yield return saveLoadService.LoadAll();
            
            yield return container.Get<YandexGameIntegrator>().Initialize();
            
            yield return container.Get<IPurchaseService>().Initialize();

            yield return new WaitForEndOfFrame();
            
            container.Get<ISoundController>().Init();
            
            SetupMediators(container);

            yield return container.Get<ISceneManager>().LoadScene(SceneIDs.MAIN_MENU);
        }

        private void SetupMediators(IDIContainer container)
        {
            CompositeDisposable contextDisposables = container.Get<CompositeDisposable>(EntryPointTags.PROJECT_DISPOSABLES_TAG);
            
            contextDisposables.Add(new SoundAndSceneChangingMediator(container));
            contextDisposables.Add(new SaveOnQuitMediator(container));
            contextDisposables.Add(new OnAdSaveTimeMediator(container));
            contextDisposables.Add(new OnAdPauseMediator(container));
            contextDisposables.Add(new CoinsLeaderboardMediator(container));
            contextDisposables.Add(new OnApplicationQuitDisposeMediator(container, EntryPointTags.PROJECT_DISPOSABLES_TAG));
            contextDisposables.Add(new DisposableDelegate(() => container.Get<ICoroutinePerformer>().StopAllCoroutines()));
        }
    }
}