
namespace YG.EditorScr.BuildModify
{
    public partial class ModifyBuildManager
    {
        public static void LoadGameRun()
        {
            indexFile = indexFile.Replace("if (LocalHost()) // Delete when setting up: Load Game Run", "// Load Game Run = true");
        }
    }
}
