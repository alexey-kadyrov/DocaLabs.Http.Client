﻿using System;
using System.Net;
using System.Threading.Tasks;
using DocaLabs.Test.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocaLabs.Http.Client.Tests
{
    [TestClass]
    public class when_asynchronously_executing_action_which_succeeds_first_time
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static string _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _result = _strategy.Execute("Hello World!", Task.FromResult).Result;
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
    public class when_asynchronously_executing_action_which_fails_all_retries
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;

            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new WebException("ConnectFailure", WebExceptionStatus.ConnectFailure);
            }).Wait());

            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_eventually_throw_original_exception()
        {
            ((AggregateException)_exception).InnerExceptions[0].ShouldBeOfType<WebException>();
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
    public class when_asynchronously_executing_action_which_fails_all_retries_on_aggregate_exception_where_all_exceptions_are_retryable
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishCointext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;

            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new AggregateException(new WebException("ConnectFailure", WebExceptionStatus.ConnectFailure), new AggregateException(new WebException("ConnectFailure", WebExceptionStatus.ConnectFailure), new WebException("ConnectFailure", WebExceptionStatus.ConnectFailure)));
            }).Wait());

            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_eventually_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<AggregateException>();
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
    public class when_asynchronously_executing_which_throws_aggregate_exception_where_not_all_exceptions_are_retryable
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new AggregateException(new TestException(), new AggregateException(new ArgumentNullException(), new TestException()));
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_eventually_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<AggregateException>();
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

        class TestException : Exception
        {
        }
    }

    [TestClass]
    public class when_asynchronously_executing_action_which_succeeds_after_retry
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static string _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(200) });

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;

            _result = _strategy.Execute("Hello World!", x =>
            {
                ++_attempts; 
                if( _attempts == 1)
                    throw new WebException("ConnectFailure", WebExceptionStatus.ConnectFailure);
                return Task.FromResult(x);
            }).Result;

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
    public class when_asynchronously_executing_action_throws_aggregate_exception_with_all_non_retryable_exceptions
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;

            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new AggregateException(new ArgumentException(), new AggregateException(new ArgumentNullException(), new ArgumentException()));
            }).Wait());

            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<AggregateException>();
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
    public class when_asynchronously_executing_action_throws_argument_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new ArgumentException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<ArgumentException>();
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
    public class when_asynchronously_executing_action_throws_exception_derived_from_argument_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new TestException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<TestException>();
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

        class TestException : ArgumentException
        {
        }
    }

    [TestClass]
    public class when_asynchronously_executing_action_throws_null_refrence_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new NullReferenceException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<NullReferenceException>();
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
    public class when_asynchronously_executing_action_throws_exception_derived_from_null_reference_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new TestException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<TestException>();
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

        class TestException : NullReferenceException
        {
        }
    }

    [TestClass]
    public class when_asynchronously_executing_action_throws_not_supported_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new NotSupportedException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<NotSupportedException>();
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
    public class when_asynchronously_executing_action_throws_exception_derived_from_not_supported_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new TestException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<TestException>();
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

        class TestException : NotSupportedException
        {
        }
    }

    [TestClass]
    public class when_asynchronously_executing_action_throws_not_implemented_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new NotImplementedException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<NotImplementedException>();
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
    public class when_asynchronously_executing_action_throws_exception_derived_from_not_implemented_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EtsblishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new TestException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<TestException>();
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

        class TestException : NotImplementedException
        {
        }
    }

    [TestClass]
    public class when_asynchronously_executing_action_throws_http_client_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new HttpClientException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<HttpClientException>();
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
    public class when_asynchronously_executing_action_throws_exception_derived_from_http_client_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1)});

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new TestException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<TestException>();
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
    public class when_asynchronously_executing_action_which_fails_using_strategy_initialized_with_null_array_of_retry_timeouts
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static int _attempts;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(null);

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new TestException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_eventually_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<TestException>();
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
    public class when_asynchronously_executing_action_which_succeeds_first_time_using_strategy_initialized_with_null_array_of_retry_timeouts
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static TimeSpan _duration;
        static string _result;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(null);

            BecauseOf();
        }

        static void BecauseOf()
        {
            var started = DateTime.UtcNow;
            _result = _strategy.Execute("Hello World!", Task.FromResult).Result;
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
    public class when_asynchronously_executing_null_action
    {
        static AsyncDefaultExecuteStrategy<string, string> _strategy;
        static Exception _exception;

        [ClassInitialize]
        public static void EstablishContext(TestContext context)
        {
            _strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

            BecauseOf();
        }

        static void BecauseOf()
        {
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", null).Wait());
        }

        [TestMethod]
        public void it_should_throw_argument_null_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<ArgumentNullException>();
        }

        [TestMethod]
        public void it_should_report_action_argument()
        {
            ((ArgumentNullException) ((AggregateException) _exception).InnerExceptions[0]).ParamName.ShouldEqual("action");
        }
    }

    [TestClass]
    public class when_asynchronously_using_derived_startgey_to_execute_action_which_succeeds_first_time
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
            _result = _strategy.Execute("Hello World!", Task.FromResult).Result;
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

        class DerivedStrategy : AsyncDefaultExecuteStrategy<string, string>
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
    public class when_asynchronously_using_derived_startgey_to_execute_action_which_fails_all_retries
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
            _exception = Catch.Exception(() => _strategy.Execute("Hello World!", x =>
            {
                ++_attempts;
                throw new NullReferenceException();
            }).Wait());
            _duration = DateTime.UtcNow - started;
        }

        [TestMethod]
        public void it_should_eventually_throw_original_exception()
        {
            ((AggregateException) _exception).InnerExceptions[0].ShouldBeOfType<NullReferenceException>();
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

        class DerivedStrategy : AsyncDefaultExecuteStrategy<string, string>
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
