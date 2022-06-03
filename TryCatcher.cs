using BenchmarkDotNet.Attributes;

namespace TryCatchBenchmark
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class TryCatcher
    {
        private const int LoopCount = 100000;

        [Benchmark(Baseline = true)]
        public void FlowWithCastCustomException3() => LoopWrapper(() =>
            FlowWithCast(new CustomException3("CustomException3")));

        [Benchmark]
        public void FlowWithStackViewCustomException3() =>
            LoopWrapper(() => FlowWithStackView(new CustomException3("CustomException3")));

        [Benchmark]
        public void FlowWithCastException() =>
            LoopWrapper(() => FlowWithCast(new Exception("SystemException")));

        [Benchmark]
        public void FlowWithStackViewException() =>
            LoopWrapper(() => FlowWithStackView(new Exception("SystemException")));

        private void LoopWrapper(Action act)
        {
            for (var i = 0; i < LoopCount; i++)
            {
                act();
            }
        }

        private void FlowWithCast<TException>(TException exception) where TException : Exception
        {
            try
            {
                throw exception;
            }
            catch (Exception e)
            {
                if (e is CustomException1)
                {
                    var t1 = 1;
                    return;
                }
                if (e is CustomException2)
                {
                    var t2 = 2;
                    return;
                }

                if (e is CustomException3)
                {
                    var t3 = 3;
                    return;
                }

                var t4 = 4;
            }
        }

        private void FlowWithStackView<TException>(TException exception) where TException : Exception
        {
            try
            {
                throw exception;
            }
            catch (CustomException1 e)
            {
                var t1 = 1;
            }
            catch (CustomException2 e)
            {
                var t2 = 2;
            }
            catch (CustomException3 e)
            {
                var t3 = 3;
            }
            catch (Exception e)
            {
                var t4 = 4;
            }
        }
    }

    internal class CustomException3 : Exception
    {
        public string NameException { get; }
        public CustomException3(string nameException)
        {
            NameException = nameException;
        }
    }

    internal class CustomException2 : Exception
    {
        public string NameException { get; }
        public CustomException2(string nameException)
        {
            NameException = nameException;
        }
    }

    internal class CustomException1 : Exception
    {
        public string NameException { get; }
        public CustomException1(string nameException)
        {
            NameException = nameException;
        }
    }
}
