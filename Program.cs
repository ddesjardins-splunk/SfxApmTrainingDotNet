using System;
using System.Threading;
using OpenTracing;
using OpenTracing.Util;
using OpenTracing.Tag;
using System.Threading.Tasks;
using System.Net.Http;

namespace SfxCurrencyConverter
{
    class Program
    {
        static ITracer s_tracer = GlobalTracer.Instance;

        class SfxCurrencyConverter
        {
            public static SfxCurrencyConverter s_instance = new SfxCurrencyConverter();


            void ConvertMyAmount(double dAmount)
            {
                using (IScope scope = s_tracer.BuildSpan("ConvertMyAmount").StartActive(finishSpanOnDispose: true))
                {
                    var span = scope.Span;

                    Console.WriteLine("Converting" + dAmount.ToString() + "To Nothing . . .");
                    Thread.Sleep(30);
                    span.SetTag("amount", dAmount.ToString());
                }
            }

            /*
            public Task<int> DoConversion(double dAmount)
            {
                Console.WriteLine("DoConversion" + dAmount.ToString());
                using (IScope scope = s_tracer.BuildSpan("DoConversion").StartActive(finishSpanOnDispose: true))
                {
                    var span = scope.Span;
                    ConvertMyAmount(dAmount);
                }
                return Task.FromResult<int>(0);
            }*/

            public void DoConversion(double dAmount)
            {
                Console.WriteLine("DoConversion" + dAmount.ToString());
                using (IScope scope = s_tracer.BuildSpan("DoConversion").StartActive(finishSpanOnDispose: true))
                {
                    var span = scope.Span;
                    ConvertMyAmount(dAmount);
                }
            }
        }

        //public static void Main(string[] args)
        public static async Task<int> Main(string[] args)
        {

            var tracer = GlobalTracer.Instance;

            Console.WriteLine(tracer.ToString());
            Console.WriteLine(GlobalTracer.IsRegistered());

        
            using (IScope scope = tracer.BuildSpan("ConversionRequestParent").StartActive(finishSpanOnDispose: false))
            {

                var span = scope.Span;

                Console.WriteLine("lets go !");
                for (int i = 0; i < 100; i++)
                {
                    span.SetTag("amount", i);
                    Thread.Sleep(30);
                    SfxCurrencyConverter.s_instance.DoConversion(i);

                    var ba = new Uri("http://localhost:5000/");
                    var c = new HttpClient { BaseAddress = ba };
                    //await c.GetAsync("default-handler");
                    await Task.Delay(100);
                }
            }
            Thread.Sleep(3000);
            return 0;
        }
    }
}


