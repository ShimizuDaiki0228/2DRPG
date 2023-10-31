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
            "敵がいるぞ、倒してみよう\nマウスの左クリックで攻撃を出すことができるぞ\n攻撃は3連続攻撃まであり、連続でクリックすることで出すことができるぞ"
        };

        public static string GetDescriptionText(int index)
        {
            return descriptionText[index];
        }

        public static List<List<string>> specialDescriptionText = new List<List<string>>()
        {
            new List<string>()
            {
                "アイテムがドロップしたみたいだ、拾ってみよう。",
            },
            new List<string>()
            {
                "このアイテムを使ってアイテムを作ってみよう",
                "Pキーをクリックして作成メニューを開いてみよう",
                "ここではアイテムを作成することができる。\n試しに先ほど手に入れたアイテムを使って武器を作ってみよう。"
            }
        };

        public static List<string> GetSpecialDescriptionText(int index)
        {
            return specialDescriptionText[index];
        }
    }

}
