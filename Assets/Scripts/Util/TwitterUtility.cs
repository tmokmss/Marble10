using naichilab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Twitterのことはまかせろー
/// </summary>
class TwitterUtility
{
    public static Director director;
    public static int ReachedLevel { set; get; }
    public static float Time { set; get; }
    //public static Scorer CurrentScore { set; get; }

    public static void Tweet()
    {
        UnityRoomTweet.Tweet("marble10", MakeTweetString(), "unityroom", "unity1week");
    }

    static string MakeTweetString()
    {
        //var CurrentScore = director.Score;
        //return $"レベル{CurrentScore.ReachedLevel}に{CurrentScore.TimeSum:f2}秒で到達しました！";
        return $"レベル{ReachedLevel}に{Time:f2}秒で到達しました！";
    }
}
