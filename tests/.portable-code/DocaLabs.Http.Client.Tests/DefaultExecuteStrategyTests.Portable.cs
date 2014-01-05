using System;
using System.Net;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests
{
    [TestClass]
    public class when_executing_action_which_succeeds_first_time
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static string _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _result = _strategy.Execute("Hello World!", x => x);
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_return_the_result_of_the_action()
        {
            _result.ShouldEqual("Hello World!");
        }

        [TestMethod]
        public void it_should_return_after_first_time_execution()
        {
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }
    }

    [TestClass]
    public class when_executing_action_which_fails_all_retries
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x => { ++_attempts; throw new WebException("Connection Failure", WebExceptionStatus.ConnectFailure); }));
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_eventually_throw_original_exception()
        {
            _exception.ShouldBeOfType<WebException>();
        }

        [TestMethod]
        public void it_should_run_all_retries()
        {
            _attempts.ShouldEqual(3);
        }

        [TestMethod]
        public void it_should_run_use_all_timeout_retries()
        {
            _duration.ShouldBeEqualOrGreaterThan(TimeSpan.FromMilliseconds(300));
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }
    }

    [TestClass]
    public class when_executing_action_which_succeeds_after_retry
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static string _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(200)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;

            _result = _strategy.Execute("Hello World!", x =>
            {
                ++_attempts; 
                if( _attempts == 1) 
                    throw new WebException("Connection Failure", WebExceptionStatus.ConnectFailure);
                return x;
            });

            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_return_the_result_of_the_action()
        {
            _result.ShouldEqual("Hello World!");
        }

        [TestMethod]
        public void it_should_run_only_two_attempts()
        {
            _attempts.ShouldEqual(2);
        }

        [TestMethod]
        public void it_should_run_use_only_first_timeout_retry()
        {
            _duration.ShouldBeEqualOrGreaterThan(TimeSpan.FromMilliseconds(100));
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }
    }

    [TestClass]
    public class when_executing_action_throws_non_web_exception
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContecxt(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x => { ++_attempts; throw new ArgumentException(); }));
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            _exception.ShouldBeOfType<ArgumentException>();
        }

        [TestMethod]
        public void it_should_not_run_retries()
        {
            _attempts.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_not_use_timeout_retries()
        {
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }
    }

    [TestClass]
    public class when_executing_action_throws_http_client_exception
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x => { ++_attempts; throw new HttpClientException(); }));
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            _exception.ShouldBeOfType<HttpClientException>();
        }

        [TestMethod]
        public void it_should_not_run_retries()
        {
            _attempts.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_not_use_timeout_retries()
        {
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }
    }

    [TestClass]
    public class when_executing_action_throws_exception_derived_from_http_client_exception
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x => { ++_attempts; throw new TestException(); }));
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            _exception.ShouldBeOfType<TestException>();
        }

        [TestMethod]
        public void it_should_not_run_retries()
        {
            _attempts.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_not_use_timeout_retries()
        {
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }

        class TestException : HttpClientException
        {
        }
    }

    [TestClass]
    public class when_executing_action_which_fails_using_strategy_initialized_with_null_array_of_retry_timeouts
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(null);

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x => { ++_attempts; throw new TestException(); }));
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_eventually_throw_original_exception()
        {
            _exception.ShouldBeOfType<TestException>();
        }

        [TestMethod]
        public void it_should_run_only_one_attempt()
        {
            _attempts.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_return_after_first_time_execution()
        {
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }

        class TestException : Exception
        {
        }
    }

    [TestClass]
    public class when_executing_action_which_succeeds_first_time_using_strategy_initialized_with_null_array_of_retry_timeouts
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static string _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(null);

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _result = _strategy.Execute("Hello World!", x => x);
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_return_the_result_of_the_action()
        {
            _result.ShouldEqual("Hello World!");
        }

        [TestMethod]
        public void it_should_return_after_first_time_execution()
        {
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }
    }

    [TestClass]
    public class when_executing_null_action
    {
        static DefaultExecuteStrategy<string, string> _strategy;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", null));
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            _exception.ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_action_argument()
        {
            ((ArgumentNullException) _exception).ParamName.ShouldEqual("action");
        }
    }

    [TestClass]
    public class when_using_derived_startgey_to_execute_action_which_succeeds_first_time
    {
        static DerivedStrategy _strategy;
        static TimeSpan _duration;
        static string _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DerivedStrategy(new[] {TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _result = _strategy.Execute("Hello World!", x => x);
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_return_the_result_of_the_action()
        {
            _result.ShouldEqual("Hello World!");
        }

        [TestMethod]
        public void it_should_return_after_first_time_execution()
        {
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void it_should_not_call_on_retrying()
        {
            _strategy.OnRetryingCalls.ShouldEqual(0);
        }

        [TestMethod]
        public void it_should_not_call_on_rethrowing()
        {
            _strategy.OnRethrowingCalls.ShouldEqual(0);
        }

        [TestMethod]
        public void it_should_not_call_can_retry()
        {
            _strategy.CanRetryCalls.ShouldEqual(0);
        }

        class DerivedStrategy : DefaultExecuteStrategy<string, string>
        {
            public int OnRetryingCalls { get; private set; }
            public int OnRethrowingCalls { get; private set; }
            public int CanRetryCalls { get; private set; }

            public DerivedStrategy(params TimeSpan[] retryTimeouts)
                : base(retryTimeouts)
            {
            }

            protected override void OnRetrying(int attempt, int maxRetries, Exception e)
            {
                OnRetryingCalls++;
            }

            protected override void OnRethrowing(int attempt, int maxRetries, Exception e)
            {
                OnRethrowingCalls++;
            }

            protected override bool CanRetry(Exception e)
            {
                CanRetryCalls++;
                return true;
            }
        }
    }

    [TestClass]
    public class when_using_derived_startgey_to_execute_action_which_fails_all_retries
    {
        static DerivedStrategy _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new DerivedStrategy(new[] {TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x => { ++_attempts; throw new NullReferenceException(); }));
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_eventually_throw_original_exception()
        {
            _exception.ShouldBeOfType<NullReferenceException>();
        }

        [TestMethod]
        public void it_should_run_all_retries()
        {
            _attempts.ShouldEqual(3);
        }

        [TestMethod]
        public void it_should_run_use_all_timeout_retries()
        {
            _duration.ShouldBeEqualOrGreaterThan(TimeSpan.FromMilliseconds(300));
            _duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void it_should_call_on_retrying_for_each_retry()
        {
            _strategy.OnRetryingCalls.ShouldEqual(2);
        }

        [TestMethod]
        public void it_should_call_on_rethrowing_only_once()
        {
            _strategy.OnRethrowingCalls.ShouldEqual(1);
        }

        [TestMethod]
        public void it_should_call_can_retry_for_each_retry()
        {
            _strategy.CanRetryCalls.ShouldEqual(2);
        }

        class DerivedStrategy : DefaultExecuteStrategy<string, string>
        {
            public int OnRetryingCalls { get; private set; }
            public int OnRethrowingCalls { get; private set; }
            public int CanRetryCalls { get; private set; }

            public DerivedStrategy(params TimeSpan[] retryTimeouts)
                : base(retryTimeouts)
            {
            }

            protected override void OnRetrying(int attempt, int maxRetries, Exception e)
            {
                OnRetryingCalls++;
            }

            protected override void OnRethrowing(int attempt, int maxRetries, Exception e)
            {
                OnRethrowingCalls++;
            }

            protected override bool CanRetry(Exception e)
            {
                CanRetryCalls++;
                return true;
            }
        }
    }
}
