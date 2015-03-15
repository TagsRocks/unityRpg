using UnityEngine;
using System.Collections;
using Xft;
using System.Collections.Generic;

namespace Xft
{
    public class CustomMesh
    {
        protected VertexPool.VertexSegment Vertexsegment;
        
        
        public Mesh MyMesh;
        
        public Vector3[] MeshVerts;

        public Color MyColor;
        public Vector3 MyPosition = Vector3.zero;
        public Vector2 MyScale = Vector2.one;
        public float ScaleZ = 1;
        public Quaternion MyRotation = Quaternion.identity;
        public  Vector3 MyDirection;
        
        
        Matrix4x4 LocalMat;
        Matrix4x4 WorldMat;
        float Fps = 0.016f;
        float ElapsedTime = 0f;
        
       // EffectNode Owner;

        public bool ColorChanged = false;
        public bool UVChanged = false;
        
        protected Vector2 LowerLeftUV;
        protected Vector2 UVDimensions;
        
        protected Vector2[] m_oriUvs = null;
        
        protected EffectNode m_owner;
        
        
        public CustomMesh(VertexPool.VertexSegment segment, Mesh mesh,Vector3 dir, float maxFps,EffectNode owner)
        {
   
            MyMesh = mesh;
            
            m_owner = owner;
            
            MeshVerts =  new Vector3[mesh.vertices.Length];
            
            mesh.vertices.CopyTo(MeshVerts,0);

            Vertexsegment = segment;
            MyDirection = dir;
            Fps = 1f / maxFps;
            SetPosition(Vector3.zero);
            InitVerts();
        }
        
        
        public void SetUVCoord(Vector2 lowerleft, Vector2 dimensions)
        {
            LowerLeftUV = lowerleft;
            UVDimensions = dimensions;
            UVChanged = true;
        }
        
        public void SetColor(Color c)
        {
            MyColor = c;
            ColorChanged = true;
        }

        public void SetPosition(Vector3 pos)
        {
            MyPosition = pos;
        }
        
        public void SetScale(float width, float height)
        {
            MyScale.x = width;
            MyScale.y = height;
        }
        
        public void SetRotation(float angle)
        {
            MyRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
 
        public void InitVerts()
        {
            VertexPool pool = Vertexsegment.Pool;
            int index = Vertexsegment.IndexStart;
            int vindex = Vertexsegment.VertStart;

            for (int i = 0; i < MeshVerts.Length; i++)
            {
                pool.Vertices[vindex + i] = MeshVerts[i];
            }
            
            
            int[] indices = MyMesh.triangles;
            for (int i = 0; i < Vertexsegment.IndexCount; i++)
            {
                pool.Indices[i + index] = indices[i] + vindex;
            }
            
            m_oriUvs = MyMesh.uv;
            for (int i = 0; i < m_oriUvs.Length; i++)
            {
                pool.UVs[i + vindex] = m_oriUvs[i];
            }
            
            Color[] colors = MyMesh.colors;
            for (int i = 0; i < colors.Length; i++)
            {
                //pool.Colors[i + vindex] = colors[i];
                pool.Colors[i + vindex] = Color.clear;
            }
        }
        
        public void UpdateUV()
        {
            VertexPool pool = Vertexsegment.Pool;
            int vindex = Vertexsegment.VertStart;
            
            for (int i = 0; i < m_oriUvs.Length; i++)
            {
                pool.UVs[i + vindex] = LowerLeftUV + Vector2.Scale(m_oriUvs[i] , UVDimensions);
            }
            
            Vertexsegment.Pool.UVChanged = true;
        }
        
        public void UpdateColor()
        {
            VertexPool pool = Vertexsegment.Pool;
            int index = Vertexsegment.VertStart;
            for (int i = 0; i < Vertexsegment.VertCount; i++)
            {
                pool.Colors[index + i] = MyColor;
            }
            Vertexsegment.Pool.ColorChanged = true;
        }
        
        
        public void Transform()
        {
            
            Quaternion rot = Quaternion.FromToRotation(Vector3.up,MyDirection);
            
            Vector3 scale = Vector3.one;
            //scale.x = scale.z = MyScale.x;
            scale.x = MyScale.x;
            scale.z = ScaleZ;
            scale.y = MyScale.y;

            LocalMat.SetTRS(Vector3.zero, rot * MyRotation, scale);
			//Effect Layer Rotation
			//Parent Xffcomponet Rotation
			Quaternion worldRot = m_owner.Owner.transform.parent.rotation;
            WorldMat.SetTRS(MyPosition, worldRot, Vector3.one);
            Matrix4x4 mat = WorldMat * LocalMat;

            VertexPool pool = Vertexsegment.Pool;

            for (int i = Vertexsegment.VertStart; i < Vertexsegment.VertStart + Vertexsegment.VertCount; i++)
            {
                pool.Vertices[i] = mat.MultiplyPoint3x4(MeshVerts[i - Vertexsegment.VertStart]);
            }
            
        }
        

        public void Update(bool force,float deltaTime)
        {
            ElapsedTime += deltaTime;
            if (ElapsedTime > Fps || force)
            {
                Transform();
                if (ColorChanged)
                    UpdateColor();
                if (UVChanged)
                    UpdateUV();
                ColorChanged = UVChanged = false;
                if (!force)
                    ElapsedTime -= Fps;
            }
        }
    }
}

