using System.Threading;
using System.Threading.Tasks;
using DocaLabs.Http.Client.Utils;
using Machine.Specifications;

namespace DocaLabs.Http.Client.Tests.Utils
{
    [Subject(typeof(TaskUtils), "RunSynchronously")]
    class when_action_is_run_synchronously
    {
        static string execution_marker;
        static Task task;

        Because of =
            () => task = TaskUtils.RunSynchronously(() => execution_marker = "executed", CancellationToken.None);

        It should_execute_action =
            () => execution_marker.ShouldEqual("executed");

        It should_return_completed_task =
            () => task.Status.ShouldEqual(TaskStatus.RanToCompletion);
    }

    [Subject(typeof(TaskUtils), "RunSynchronously")]
    class when_action_is_asked_to_run_synchronously_but_cancellation_token_is_in_cancelled_state
    {
        static string execution_marker;
        static Task task;

        Because of =
            () => task = TaskUtils.RunSynchronously(() => execution_marker = "executed", new CancellationToken(true));

        It should_not_execute_action =
            () => execution_marker.ShouldBeNull();

        It should_return_cancelled_task =
            () => task.Status.ShouldEqual(TaskStatus.Canceled);
    }

    [Subject(typeof(TaskUtils), "CompletedTask")]
    class when_returns_completed_task
    {
        static Task task;

        Because of =
            () => task = TaskUtils.CompletedTask();

        It should_return_completed_task =
            () => task.Status.ShouldEqual(TaskStatus.RanToCompletion);
    }
}
