using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Definition
{
    public class DescriptionDefinition
    {
        public static List<string> descriptionText = new List<string>()
        {
            "ここは魔王に支配された世界、あなたにはこの世界を救ってほしい\n操作方法はMキーをクリックすることで確認できます。",
            "そこに階段があるね、そのまま突き進むことはできないから\nスペースキーをクリックしてジャンプしていこう。",
            "いい調子だね、敵の攻撃をよける時にも使えるのでうまく利用していこう。",
            "敵がいるぞ、倒してみよう\nマウスの左クリックで攻撃を出すことができるぞ\n攻撃は3連続攻撃まであり、連続でクリックすることで出すことができるぞ",
            "ここはなんだかさっきとは明らかに違う気配がしている......\n\n見て！あそこに大きな敵がいるよ、頑張って倒そう！"
        };

        public static string GetDescriptionText(int index)
        {
            return descriptionText[index];
        }

        public static List<Dictionary<int, string>> specialDescriptionText = new List<Dictionary<int, string>>()
        {
            new Dictionary< int, string >()
            {
                {0 , "アイテムがドロップしたみたいだ、拾ってみよう。"},
            },
            new Dictionary<int, string>()
            {
                { 0 , "このアイテムを使ってアイテムを作ってみよう" },
                { 1 , "Pキーをクリックして作成メニューを開いてみよう"},
                { 2 , "ここではアイテムを作成することができる。\n試しに先ほど手に入れたアイテムを使って武器を作ってみよう。"}
            }
        };

        public static List<Dictionary<int, string>> specialDescriptionKeyInstruction = new List<Dictionary<int, string>>()
        {
            new Dictionary< int, string >()
            {
                {0 , "Enter"},
            },
            new Dictionary<int, string>()
            {
                { 0 , "Enter" },
                { 1 , "P"},
                { 2 , "Enter"}
            }
        };

        public static List<Dictionary<int, KeyCode>> specialDescriptionKeyCode = new List<Dictionary<int, KeyCode>>()
        {
            new Dictionary< int, KeyCode >()
            {
                {0 , KeyCode.P},
            },
            new Dictionary<int, KeyCode>()
            {
                { 0 , KeyCode.Return},
                { 1 , KeyCode.P},
                { 2 , KeyCode.Return}
            }
        };

        public static Dictionary<int, string> GetSpecialDescriptionText(int index)
        {
            return specialDescriptionText[index];
        }

        public static Dictionary<int, string> GetSpecialDescriptionKeyInstruction(int index)
        {
            return specialDescriptionKeyInstruction[index];
        }

        public static Dictionary<int, KeyCode> GetSpecialDescriptionKeyCode(int index)
        {
            return specialDescriptionKeyCode[index];
        }
    }

}
