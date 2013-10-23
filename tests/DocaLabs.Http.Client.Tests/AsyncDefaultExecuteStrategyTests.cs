﻿using System;
using System.Threading.Tasks;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests
{
    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_which_succeeds_first_time
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static string result;

        Establish context = 
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] {TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)});

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            result = strategy.Execute("Hello World!", Task.FromResult).Result;
            duration = DateTime.UtcNow - started;
        };

        It should_return_the_result_of_the_action =
            () => result.ShouldEqual("Hello World!");

        It should_return_after_first_time_execution =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_which_fails_all_retries
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new TestException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_eventually_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<TestException>();

        It should_run_all_retries =
            () => attempts.ShouldEqual(3);

        It should_run_use_all_timeout_retries =
            () => duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(300)).ShouldBeLessThan(TimeSpan.FromSeconds(1));

        class TestException : Exception
        {
        }
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_which_succeeds_after_retry
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(200) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;

            result = strategy.Execute("Hello World!", x =>
            {
                ++attempts; 
                if( attempts == 1) 
                    throw new TestException();
                return Task.FromResult(x);
            }).Result;

            duration = DateTime.UtcNow - started;
        };

        It should_return_the_result_of_the_action =
            () => result.ShouldEqual("Hello World!");

        It should_run_only_two_attempts =
            () => attempts.ShouldEqual(2);

        It should_run_use_only_first_timeout_retry =
            () => duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(100)).ShouldBeLessThan(TimeSpan.FromSeconds(1));

        class TestException : Exception
        {
        }
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_argument_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new ArgumentException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<ArgumentException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_exception_derived_from_argument_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new TestException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<TestException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));

        class TestException : ArgumentException
        {
        }
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_null_refrence_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new NullReferenceException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<NullReferenceException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_exception_derived_from_null_reference_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new TestException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<TestException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));

        class TestException : NullReferenceException
        {
        }
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_not_supported_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new NotSupportedException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<NotSupportedException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_exception_derived_from_not_supported_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new TestException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<TestException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));

        class TestException : NotSupportedException
        {
        }
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_not_implemented_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new NotImplementedException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<NotImplementedException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_exception_derived_from_not_implemented_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new TestException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<TestException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));

        class TestException : NotImplementedException
        {
        }
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_http_client_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new HttpClientException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<HttpClientException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_throws_exception_derived_from_http_client_exception
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new TestException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<TestException>();

        It should_not_run_retries =
            () => attempts.ShouldEqual(1);

        It should_not_use_timeout_retries =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));

        class TestException : HttpClientException
        {
        }
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_which_fails_using_strategy_initialized_with_null_array_of_retry_timeouts
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(null);

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new TestException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_eventually_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<TestException>();

        It should_run_only_one_attempt =
            () => attempts.ShouldEqual(1);

        It should_return_after_first_time_execution =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));

        class TestException : Exception
        {
        }
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_action_which_succeeds_first_time_using_strategy_initialized_with_null_array_of_retry_timeouts
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static TimeSpan duration;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(null);

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            result = strategy.Execute("Hello World!", Task.FromResult).Result;
            duration = DateTime.UtcNow - started;
        };

        It should_return_the_result_of_the_action =
            () => result.ShouldEqual("Hello World!");

        It should_return_after_first_time_execution =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>))]
    class when_asynchronously_executing_null_action
    {
        static AsyncDefaultExecuteStrategy<string, string> strategy;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new AsyncDefaultExecuteStrategy<string, string>(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

        Because of =
            () => exception = Catch.Exception(() => { result = strategy.Execute("Hello World!", null).Result; });

        It should_throw_argument_null_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<ArgumentNullException>();

        It should_report_action_argument =
            () => ((ArgumentNullException)((AggregateException)exception).InnerExceptions[0]).ParamName.ShouldEqual("action");
    }

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>), "inheritable behavior")]
    class when_asynchronously_using_derived_startgey_to_execute_action_which_succeeds_first_time
    {
        static DerivedStrategy strategy;
        static TimeSpan duration;
        static string result;

        Establish context =
            () => strategy = new DerivedStrategy(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            result = strategy.Execute("Hello World!", Task.FromResult).Result;
            duration = DateTime.UtcNow - started;
        };

        It should_return_the_result_of_the_action =
            () => result.ShouldEqual("Hello World!");

        It should_return_after_first_time_execution =
            () => duration.ShouldBeLessThan(TimeSpan.FromSeconds(1));

        It should_not_call_on_retrying =
            () => strategy.OnRetryingCalls.ShouldEqual(0);

        It should_not_call_on_rethrowing =
            () => strategy.OnRethrowingCalls.ShouldEqual(0);

        It should_not_call_can_retry =
            () => strategy.CanRetryCalls.ShouldEqual(0);

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

    [Subject(typeof(AsyncDefaultExecuteStrategy<,>), "inheritable behavior")]
    class when_asynchronously_using_derived_startgey_to_execute_action_which_fails_all_retries
    {
        static DerivedStrategy strategy;
        static TimeSpan duration;
        static int attempts;
        static Exception exception;
        static string result;

        Establish context =
            () => strategy = new DerivedStrategy(new[] { TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(200) });

        Because of = () =>
        {
            var started = DateTime.UtcNow;
            exception = Catch.Exception(() =>
            {
                result = strategy.Execute("Hello World!", x =>
                {
                    ++attempts;
                    throw new NullReferenceException();
                }).Result;
            });
            duration = DateTime.UtcNow - started;
        };

        It should_eventually_throw_original_exception =
            () => ((AggregateException)exception).InnerExceptions[0].ShouldBeOfType<NullReferenceException>();

        It should_run_all_retries =
            () => attempts.ShouldEqual(3);

        It should_run_use_all_timeout_retries =
            () => duration.ShouldBeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(300)).ShouldBeLessThan(TimeSpan.FromSeconds(1));

        It should_call_on_retrying_for_each_retry =
            () => strategy.OnRetryingCalls.ShouldEqual(2);

        It should_call_on_rethrowing_only_once =
            () => strategy.OnRethrowingCalls.ShouldEqual(1);

        It should_call_can_retry_for_each_retry =
            () => strategy.CanRetryCalls.ShouldEqual(2);

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