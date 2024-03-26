using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MudaLifecanManuelFocus
{
    internal class Program
    {
        private const string CAMERA_NAME = "Microsoft® LifeCam";

        static void Main(string[] args)
        {
            // Gets the device list
            DsDevice[] devs = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            IAMCameraControl lifecam = GetCamera(devs.Where(d => d.Name.ToLower().Contains(CAMERA_NAME.ToLower())).FirstOrDefault());

            if (lifecam != null)
            {
                Console.WriteLine("Microsft LifeCam detected!");
            }
            else
            {
                Console.WriteLine("Microsoft LifeCam not detect. Closing...");
                return;
            }

            lifecam.Get(CameraControlProperty.Focus, out int v, out CameraControlFlags f);

            if (CameraControlFlags.Manual != f)
            {
                lifecam.Set(CameraControlProperty.Focus, v, CameraControlFlags.Manual);
                Console.WriteLine($"{CAMERA_NAME} {CameraControlProperty.Focus} set to {CameraControlFlags.Manual}");
            }
            else
            {
                Console.WriteLine($"{CAMERA_NAME} {CameraControlProperty.Focus} already {CameraControlFlags.Manual}");
            }
        }

        static IAMCameraControl GetCamera(DsDevice dev)
        {
            IAMCameraControl _camera = null;
            if (dev != null)
            {
                // DirectShow uses a module system called filters to exposure the functionality
                // We create a new object that implements the IFilterGraph2 interface so that we can
                // new filters to exposure the functionality that we need.
                if (new FilterGraph() is IFilterGraph2 graphBuilder)
                {
                    // Create a video capture filter for the device
                    graphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out IBaseFilter capFilter);

                    // Cast that filter to IAMCameraControl from the DirectShowLib
                    _camera = capFilter as IAMCameraControl;
                }
            }
            return _camera;
        }
    }
}
