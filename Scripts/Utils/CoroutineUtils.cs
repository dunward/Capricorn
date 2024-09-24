using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEngine;

public static class CoroutineUtils
{
    public static IEnumerator AsCoroutine(this Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            throw task.Exception;
        }
    }
    
    public static Task AsTask(this IEnumerator enumerator, MonoBehaviour monoBehaviour)
    {
        var tcs = new TaskCompletionSource<bool>();
        monoBehaviour.StartCoroutine(RunEnumerator(enumerator, tcs));
        return tcs.Task;
    }

    private static IEnumerator RunEnumerator(IEnumerator enumerator, TaskCompletionSource<bool> tcs)
    {
        while (true)
        {
            try
            {
                if (!enumerator.MoveNext())
                {
                    tcs.SetResult(true);
                    yield break;
                }
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                yield break;
            }
            yield return enumerator.Current;
        }
    }
}