using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game1
{
    public class Camera
    {
        CameraAula cameraAula;
        CameraSurfaceFollow cameraSurface;
        CameraTank cameraTank;
        int alturaMapa;
        VertexPositionNormalTexture[] vertices;
        Tank tank;
        

       
        enum CameraAtiva
        {
            fps,
            free,
            cameraTank
        };
        public void Initialize(GraphicsDeviceManager graphics,VertexPositionNormalTexture[]vertices,int alturaMapa)
        {
            

            this.alturaMapa=alturaMapa;
            this.vertices=vertices;

            cameraAula = new CameraAula(graphics);
            cameraSurface = new CameraSurfaceFollow(graphics, vertices, alturaMapa);
            cameraTank = new CameraTank(graphics, vertices, alturaMapa, tank.position, tank.world, tank.view);

            UpdateViewMatrix();
            
         
    
        }

        private void UpdateViewMatrix()
        {
            cameraAula.updateCamera();
            cameraTank.updateCamera(tank.position, tank.world, tank.view, tank);
            cameraSurface.updateCamera();


            
            
        }

        static private void UpdateInput()
        {
            
        }

        

        static public void Update()
        {
         
        }

    }
}