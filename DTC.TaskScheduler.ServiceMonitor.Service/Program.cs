namespace DTC.OEM.EDF.ServiceMonitor.Service
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);
            var thisService = new Service1();
            thisService.OnStartAsync();
            while (true)
            {

            }
        }
    }
}
