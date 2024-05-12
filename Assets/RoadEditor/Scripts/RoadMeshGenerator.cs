using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode(), RequireComponent(typeof(SplineContainer),typeof(MeshFilter),typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider))]
public class RoadMeshGenerator: MonoBehaviour
{
    #if UNITY_EDITOR

    private const string ROAD_PREFAB_PATH = "Assets/RoadEditor/Prefabs/Road.prefab";
    private const string NEW_ROADS_SAVE_PATH = "Assets/Levels/RoadPrefabs";

    [SerializeField, Min(5)] private int _resolution = 5;
   
    [SerializeField, Min(0)] private float _sideOffset;
    private float3 _position;
    private float3 _forward;
    private float3 _upVector;
    private SplineContainer _splineContainer;
    private MeshFilter _meshFilter;
    private MeshCollider _collider;

    private List<Vector3> _verticiesLeft = new List<Vector3>();
    private List<Vector3> _verticiesRight= new List<Vector3>();

    private void OnEnable() 
    {
        _splineContainer = GetComponent<SplineContainer>();
        _meshFilter = GetComponent<MeshFilter>();
        _collider = GetComponent<MeshCollider>();

        transform.position = Vector3.zero;

        BuildRoad();

        if (Application.isPlaying)
            this.enabled = false;
    }

    private void Update() 
    {
        if (Application.isPlaying)
            return;

        BuildRoad();
    }

    private void BuildRoad()
    {
        _verticiesLeft.Clear();
        _verticiesRight.Clear();

        

        if (_splineContainer.Splines.Count == 0)
            return;

        if (_splineContainer.Spline.Knots.Count() < 2)
            return;

        float step = 1f/_resolution;
        

        for (int i = 0; i<_resolution;i++)
        {
            float time = i*step;
            _splineContainer.Evaluate(time, out _position, out _forward, out _upVector);
            float3 offset = Vector3.Cross(_forward,_upVector).normalized*_sideOffset;
            _verticiesLeft.Add(_position+offset);
            _verticiesRight.Add(_position-offset);
        }

        BuildMesh();
    }
    
    [MenuItem("GameObject/Road/Create new road", false, 10)]
    public static void CreateNewRoad(MenuCommand menuCommand)
    {
        Object originalPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(ROAD_PREFAB_PATH, typeof(GameObject));
        GameObject objSource = PrefabUtility.InstantiatePrefab(originalPrefab) as GameObject;   
        var assets = AssetDatabase.FindAssets("", new string[] {NEW_ROADS_SAVE_PATH});
        PrefabUtility.SaveAsPrefabAsset(objSource, $"{NEW_ROADS_SAVE_PATH}/Level road {assets.Length + 1}.prefab"); 

        DestroyImmediate(objSource);
        
        Object newPrefab = (GameObject)AssetDatabase.LoadAssetAtPath($"{NEW_ROADS_SAVE_PATH}/Level road {assets.Length + 1}.prefab", typeof(GameObject));
        GameObject newObj = PrefabUtility.InstantiatePrefab(newPrefab) as GameObject;   
    }

    private void OnDrawGizmos() 
    {
        foreach (Vector3 position in _verticiesLeft)
        {
            Gizmos.DrawSphere(position,0.2f);
        }

        foreach (Vector3 position in _verticiesRight)
        {
            Gizmos.DrawSphere(position,0.2f);
        }
    }

    private void BuildMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verticies = new List<Vector3>();
        List<int>triangles = new List<int>();

        int offset = 0;

        for (int i = 1; i<=_verticiesRight.Count;i++)
        {
            Vector3 p1 = _verticiesLeft[i-1];
            Vector3 p2 = _verticiesRight[i-1];
            Vector3 p3;
            Vector3 p4;

            if (i == _verticiesLeft.Count)
            {
                p3 = _verticiesLeft[0];
                p4 = _verticiesRight[0];
            }
            else
            {
                p3 = _verticiesLeft[i];
                p4 = _verticiesRight[i];
            }

            offset = 4*(i-1);

            int t1 = offset;
            int t2 = offset+2;
            int t3 = offset+3;

            int t4 = offset+3;
            int t5 = offset+1;
            int t6 = offset;

            verticies.AddRange(new Vector3[]{p1,p2,p3,p4});
            triangles.AddRange(new int[]{t1,t2,t3,t4,t5,t6});
        }

        mesh.SetVertices(verticies);
        mesh.SetTriangles(triangles,0);


        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        _meshFilter.mesh = mesh;
        _collider.sharedMesh = mesh;

        #endif
    }
}
