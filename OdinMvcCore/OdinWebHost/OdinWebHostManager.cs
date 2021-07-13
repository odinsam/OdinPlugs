using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
/*
使用需要将main函数  CreateHostBuilder(args).Build().Run();  修改为:
try
{
    var odinWebHostManager = OdinWebHostManager.Load();
    do
    {
        odinWebHostManager.Start<Startup>(CreateHostBuilder(args));
    } while (odinWebHostManager.Restarting);
}
catch (Exception ex)
{

}
*/
namespace OdinPlugs.OdinMvcCore.OdinWebHost
{
    public class OdinWebHostManager
    {
        private static OdinWebHostManager _appManager;
        private CancellationTokenSource _tokenSource;
        private bool _running;
        private bool _restart;

        public bool Restarting => _restart;

        public OdinWebHostManager()
        {
            _running = false;
            _restart = false;

        }

        public static OdinWebHostManager Load()
        {
            if (_appManager == null)
                _appManager = new OdinWebHostManager();

            return _appManager;
        }

        public void Start<TStartup>(IHostBuilder builder) where TStartup : class
        {
            if (_running)
                return;

            if (_tokenSource != null && _tokenSource.IsCancellationRequested)
                return;

            _tokenSource = new CancellationTokenSource();
            _tokenSource.Token.ThrowIfCancellationRequested();
            _running = true;

            builder.Build().RunAsync(_tokenSource.Token).Wait();
        }

        public void Stop()
        {
            if (!_running)
                return;

            _tokenSource.Cancel();
            _running = false;
        }

        public void Restart()
        {
            Stop();

            _restart = true;
            _tokenSource = null;
        }
    }
}