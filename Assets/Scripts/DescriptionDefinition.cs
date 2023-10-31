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
            "�G�����邼�A�|���Ă݂悤\n�}�E�X�̍��N���b�N�ōU�����o�����Ƃ��ł��邼\n�U����3�A���U���܂ł���A�A���ŃN���b�N���邱�Ƃŏo�����Ƃ��ł��邼"
        };

        public static string GetDescriptionText(int index)
        {
            return descriptionText[index];
        }

        public static List<List<string>> specialDescriptionText = new List<List<string>>()
        {
            new List<string>()
            {
                "�A�C�e�����h���b�v�����݂������A�E���Ă݂悤�B",
            },
            new List<string>()
            {
                "���̃A�C�e�����g���ăA�C�e��������Ă݂悤",
                "P�L�[���N���b�N���č쐬���j���[���J���Ă݂悤",
                "�����ł̓A�C�e�����쐬���邱�Ƃ��ł���B\n�����ɐ�قǎ�ɓ��ꂽ�A�C�e�����g���ĕ��������Ă݂悤�B"
            }
        };

        public static List<string> GetSpecialDescriptionText(int index)
        {
            return specialDescriptionText[index];
        }
    }

}
