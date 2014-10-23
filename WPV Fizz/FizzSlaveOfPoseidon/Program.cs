using LeagueSharp;
using LeagueSharp.Common;
namespace WPVFizz
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += WPVFizz.OnGameLoad;
        }
       
    }
}
