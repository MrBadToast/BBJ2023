using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum StatusType
{
    None = 0,
    /// <summary>
    /// ü��
    /// </summary>
    HP = 100,
    /// <summary>
    /// �ִ� ü��
    /// </summary>
    MaxHP = 101,
    /// <summary>
    /// ü�� ȸ����
    /// </summary>
    RecoverHP = 102,
    /// <summary>
    /// ü�� �����
    /// </summary>
    AbsorptionHP = 103,

    /// <summary>
    /// ����
    /// </summary>
    MP = 200,
    /// <summary>
    /// �ִ� ����
    /// </summary>
    MaxMP = 201,
    /// <summary>
    /// ���� ȸ����
    /// </summary>
    RecoverMP = 202,
    /// <summary>
    /// ���� �����
    /// </summary>
    AbsorptionMP = 203,
    /// <summary>
    /// ���� �����
    /// </summary>
    SkillSpendMP = 204,


    /// <summary>
    /// �ּ� ���ݷ�
    /// </summary>
    MinAttackPower = 300,
    /// <summary>
    /// �ִ� ���ݷ�
    /// </summary>
    MaxAttackPower = 301,
    /// <summary>
    /// ��ų ���ݷ�
    /// </summary>
    SkillAttackPower = 302,

    /// <summary>
    /// ġ��Ÿ Ȯ��
    /// </summary>
    CriticalPercent = 400,
    /// <summary>
    /// ġ��Ÿ ���ݷ�
    /// </summary>
    CriticalAttackPower = 401,


    /// <summary>
    /// ����
    /// </summary>
    Defence = 500,
    /// <summary>
    /// ���� �̻� ����
    /// </summary>
    DefenceCrowd = 501,
    /// <summary>
    /// �Ӽ� ����
    /// </summary>
    DefenceElemental = 502,

    /// <summary>
    /// ���� �ӵ�
    /// </summary>
    AttackSpeed = 600,
    /// <summary>
    /// ���� ��Ÿ�
    /// </summary>
    AttackDistance = 601,

    /// <summary>
    /// �̵� �ӵ�
    /// </summary>
    MoveSpeed = 700,
    /// <summary>
    /// ������
    /// </summary>
    JumpPower = 701,
    /// <summary>
    /// ������
    /// </summary>
    SkillCoolTime = 702,

    /// <summary>
    /// �ǰ�
    /// </summary>
    Health = 800,
    /// <summary>
    /// �ִ� �ǰ�
    /// </summary>
    MaxHealth = 801,

    /// <summary>
    /// �����
    /// </summary>
    Condition = 900,
    /// <summary>
    /// �ִ� �����
    /// </summary>
    MaxCondition = 901,
}
