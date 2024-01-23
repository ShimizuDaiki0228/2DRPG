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
                { 2 , "ここではアイテムを作成することができる。\n\n試しに先ほど手に入れたアイテムを使って武器を作ってみよう。"}
            },
            new Dictionary<int, string>()
            {
                {0, "いいね！　新しい装備を作れたら次はOキーをクリックして装備してみよう!" },
                {1, "ここで先ほど作成した装備を装着することができるよ！\n\n画面右下にある装備をクリックしてみよう" }
            },
            new Dictionary<int, string>()
            {
                {0, "装備できたようだね！\n\n装備の種類は4種類あるからいろんな装備を身に付けて強化しよう！" },
                {1,  "装備を身に付けることによって上がるパラメータは左で確認できるよ！\n\nマウスを近づけることで詳細説明が表示されるよ！"},
                {2, "次にスキルを確認しよう！\nスキルは敵を倒した時に得られるお金を使って開放することができるよ！\n\nKキーをクリックしてスキルを見てみよう" },
                {3, "開けたみたいだね！\n\nじゃあ、試しに一つスキルを開放してみよう！" }
            },
            new Dictionary<int, string>()
            {
                {0, "スキルが開放できたみたいだね!\n\n今回開放したスキルはマウスを右クリック長押しして離すと剣を投げて攻撃することができるよ！" },
                {1, "ちなみに一部のスキルは派生前のスキルを開放していないと開放することができないから注意しよう！" },
                {2, "最後に効果音の設定をしよう！\n\nLキーをクリックして設定画面を開いてみよう！" },
                {3, "開けたみたいだね！\nバーを調整することで音の大きさを調整することができるからもし変更したかったら変更してね！" }
            },
            new Dictionary<int, string>()
            {
                {0, "これでチュートリアルは終わりだよ！\n\nさぁ！魔王を倒しに行こう！" }
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
            },
            new Dictionary<int, string>()
            {
                {0, "O" },
                {1, "Enter" },
            },
            new Dictionary<int, string>()
            {
                {0, "Enter" },
                {1, "Enter" },
                {2, "K" },
                {3, "Enter" }
            },
            new Dictionary<int, string>()
            {
                {0, "Enter" },
                {1, "Enter" },
                {2, "L" },
                {3, "Enter" }
            },
            new Dictionary<int, string>()
            {
                {0, "Enter" }
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
            },
            new Dictionary<int, KeyCode>()
            {
                {0, KeyCode.O },
                {1, KeyCode.Return },
            },
            new Dictionary<int, KeyCode>()
            {
                {0, KeyCode.Return },
                {1, KeyCode.Return },
                {2, KeyCode.K },
                {3, KeyCode.Return }
            },
            new Dictionary<int, KeyCode>()
            {
                {0, KeyCode.Return },
                {1, KeyCode.Return },
                {2, KeyCode.L },
                {3, KeyCode.Return }
            },
            new Dictionary<int, KeyCode>()
            {
                {0, KeyCode.Return }
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
