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

        public static void Main(string[] args)
        { 
            Console.WriteLine(s_tracer.ToString());
            //Console.WriteLine(GlobalTracer.IsRegistered());

            using (IScope scope = s_tracer.BuildSpan("SfxCurrencyConverter").StartActive(finishSpanOnDispose: true))
            {
             
                var span = scope.Span;
             
                Console.WriteLine("lets go !");
                for (int i = 0; i < 100; i++)
                {
                    span.SetTag("Amount to Convert is: ", i);
                    SfxCurrencyConverter.s_instance.DoConversion(i);
                    Thread.Sleep(30);
                }
            }
            Thread.Sleep(3000);
            
        }
    }
}
