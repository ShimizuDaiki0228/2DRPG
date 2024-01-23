using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Definition
{
    public class DescriptionDefinition
    {
        public static List<string> descriptionText = new List<string>()
        {
            "�����͖����Ɏx�z���ꂽ���E�A���Ȃ��ɂ͂��̐��E���~���Ăق���\n������@��M�L�[���N���b�N���邱�ƂŊm�F�ł��܂��B",
            "�����ɊK�i������ˁA���̂܂ܓ˂��i�ނ��Ƃ͂ł��Ȃ�����\n�X�y�[�X�L�[���N���b�N���ăW�����v���Ă������B",
            "�������q���ˁA�G�̍U�����悯�鎞�ɂ��g����̂ł��܂����p���Ă������B",
            "�G�����邼�A�|���Ă݂悤\n�}�E�X�̍��N���b�N�ōU�����o�����Ƃ��ł��邼\n�U����3�A���U���܂ł���A�A���ŃN���b�N���邱�Ƃŏo�����Ƃ��ł��邼",
            "�����͂Ȃ񂾂��������Ƃ͖��炩�ɈႤ�C�z�����Ă���......\n\n���āI�������ɑ傫�ȓG�������A�撣���ē|�����I"
        };

        public static string GetDescriptionText(int index)
        {
            return descriptionText[index];
        }

        public static List<Dictionary<int, string>> specialDescriptionText = new List<Dictionary<int, string>>()
        {
            new Dictionary< int, string >()
            {
                {0 , "�A�C�e�����h���b�v�����݂������A�E���Ă݂悤�B"},
            },
            new Dictionary<int, string>()
            {
                { 0 , "���̃A�C�e�����g���ăA�C�e��������Ă݂悤" },
                { 1 , "P�L�[���N���b�N���č쐬���j���[���J���Ă݂悤"},
                { 2 , "�����ł̓A�C�e�����쐬���邱�Ƃ��ł���B\n�����ɐ�قǎ�ɓ��ꂽ�A�C�e�����g���ĕ��������Ă݂悤�B"}
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
