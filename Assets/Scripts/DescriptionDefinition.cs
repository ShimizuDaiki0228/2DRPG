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
                { 2 , "�����ł̓A�C�e�����쐬���邱�Ƃ��ł���B\n\n�����ɐ�قǎ�ɓ��ꂽ�A�C�e�����g���ĕ��������Ă݂悤�B"}
            },
            new Dictionary<int, string>()
            {
                {0, "�����ˁI�@�V������������ꂽ�玟��O�L�[���N���b�N���đ������Ă݂悤!" },
                {1, "�����Ő�قǍ쐬���������𑕒����邱�Ƃ��ł����I\n\n��ʉE���ɂ��鑕�����N���b�N���Ă݂悤" }
            },
            new Dictionary<int, string>()
            {
                {0, "�����ł����悤���ˁI\n\n�����̎�ނ�4��ނ��邩�炢���ȑ�����g�ɕt���ċ������悤�I" },
                {1,  "������g�ɕt���邱�Ƃɂ���ďオ��p�����[�^�͍��Ŋm�F�ł����I\n\n�}�E�X���߂Â��邱�Ƃŏڍא������\��������I"},
                {2, "���ɃX�L�����m�F���悤�I\n�X�L���͓G��|�������ɓ����邨�����g���ĊJ�����邱�Ƃ��ł����I\n\nK�L�[���N���b�N���ăX�L�������Ă݂悤" },
                {3, "�J�����݂������ˁI\n\n���Ⴀ�A�����Ɉ�X�L�����J�����Ă݂悤�I" }
            },
            new Dictionary<int, string>()
            {
                {0, "�X�L�����J���ł����݂�������!\n\n����J�������X�L���̓}�E�X���E�N���b�N���������ė����ƌ��𓊂��čU�����邱�Ƃ��ł����I" },
                {1, "���Ȃ݂Ɉꕔ�̃X�L���͔h���O�̃X�L�����J�����Ă��Ȃ��ƊJ�����邱�Ƃ��ł��Ȃ����璍�ӂ��悤�I" },
                {2, "�Ō�Ɍ��ʉ��̐ݒ�����悤�I\n\nL�L�[���N���b�N���Đݒ��ʂ��J���Ă݂悤�I" },
                {3, "�J�����݂������ˁI\n�o�[�𒲐����邱�Ƃŉ��̑傫���𒲐����邱�Ƃ��ł��邩������ύX������������ύX���ĂˁI" }
            },
            new Dictionary<int, string>()
            {
                {0, "����Ń`���[�g���A���͏I��肾��I\n\n�����I������|���ɍs�����I" }
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
