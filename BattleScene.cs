using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene : MonoBehaviour
{
    public static string winner;
    public fList Func;
    public Player[] player = new Player[2];
    public int[] field = new int[2], atflag = new int[2], spflag = new int[2];
    public int eff;
    float time;
    public float waitS;
    public bool liston = false;
    bool timeron = false;

    [SerializeField] GameObject[] handimg = new GameObject[6];
    public GameObject message;
    public GameObject timer;
    public GameObject[] handsta = new GameObject[6];
    public GameObject[] fieldsta = new GameObject[2];
    public GameObject[] splist = new GameObject[3];
    public GameObject[] spimg = new GameObject[8];
    public GameObject[] blindsp = new GameObject[2];

    AudioSource audiosource;
    public AudioClip select;
    public AudioClip appear;
    public AudioClip attack;
    public AudioClip genesp;
    public AudioClip handgun;
    public AudioClip heal;
    public AudioClip down;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();

        for(int i = 0; i < 2; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                player[i].hands[j].sp = player[0].hands[0].splist[j, BuildScene.selectsp[i, j] - 1];
                handimg[3 * i + j].SetActive(false);
            }
        }

        fieldsta[0].SetActive(false);
        fieldsta[1].SetActive(false);
        splist[0].SetActive(false);
        splist[1].SetActive(false);
        splist[2].SetActive(false);
        for(int i = 0; i < 8; i++)
        {
            spimg[i].SetActive(false);
        }
        timer.SetActive(false);

        StartCoroutine(Battle());
    }



    IEnumerator Battle()
    {
        yield return null;

        //初手選択
        for (int i = 0; i < 2; i++)
        {
            while (true)
            {
                message.GetComponent<Text>().text = player[i].name + "のターン：出す手を選択してください";
                yield return new WaitUntil(() => Input.anyKeyDown);
                field[i] = Func.GetCommand() - 1;
                if (field[i] < 3)
                {
                    message.GetComponent<Text>().text = "OK";
                    audiosource.PlayOneShot(select);
                    yield return new WaitForSeconds(1.0f);
                    break;
                }
                else if(field[i] == 7)
                {
                    yield return null;
                }
                else
                {
                    message.GetComponent<Text>().text = "未対応の入力です";
                    yield return new WaitForSeconds(2.0f);
                }
            }
        }



        handimg[field[0]].SetActive(true);
        message.GetComponent<Text>().text = player[0].name + "は" + player[0].hands[field[0]].name + "を繰り出した！";
        audiosource.PlayOneShot(appear);
        yield return new WaitForSeconds(2.0f);
        handimg[3 + field[1]].SetActive(true);
        message.GetComponent<Text>().text = player[1].name + "は" + player[1].hands[field[1]].name + "を繰り出した！";
        audiosource.PlayOneShot(appear);
        yield return new WaitForSeconds(2.0f);
        fieldsta[0].SetActive(true);
        fieldsta[1].SetActive(true);



        //バトル開始
        while (true)
        {
            handimg[field[0]].SetActive(true);
            handimg[3 + field[1]].SetActive(true);
            fieldsta[0].SetActive(true);
            fieldsta[1].SetActive(true);
            for(int i = 0; i < 2; i++)
            {
                if (player[i].hands[field[i]].sp == 0) blindsp[i].SetActive(true);
                else blindsp[i].SetActive(false);
            }

            int rnd;
            for (int i = 0; i < 2; i++)
            {
                //行動選択
                while (true)
                {
                    message.GetComponent<Text>().text = player[i].name + "のターン：行動を選択してください";
                    if (!timeron)
                    {
                        time = 25;
                        timeron = true;
                    }
                    yield return new WaitUntil(() => (Input.anyKeyDown || time <= 0));
                    player[i].action = Func.GetCommand();
                    if(time <= 0)
                    {
                        player[i].action = field[i] + 1;
                        message.GetComponent<Text>().text = "OK";
                        audiosource.PlayOneShot(select);
                        timeron = false;
                        timer.SetActive(false);
                        yield return new WaitForSeconds(1.0f);
                        break;
                    }
                    if (player[i].action < 5 && !(player[i].action == 4 &&(player[i].hands[field[i]].sp == 0 || player[i].hands[field[i]].sp == 1)))
                    {
                        message.GetComponent<Text>().text = "OK";
                        audiosource.PlayOneShot(select);
                        timeron = false;
                        timer.SetActive(false);
                        yield return new WaitForSeconds(1.0f);
                        break;
                    }
                    else if(player[i].action == 8)
                    {
                        yield return null;
                    }
                    else
                    {
                        message.GetComponent<Text>().text = "未対応の入力です";
                        yield return new WaitForSeconds(1.5f);
                    }
                }
            }

            atflag = new int[2] { 1, 1 };
            spflag = new int[2] { 0, 0 };

            //スペシャルチェック
            for (int i = 0; i < 2; i++)
            {
                if (player[i].action == 4)
                {
                    spflag[i] = 1;
                }
            }

            //交代処理
            for (int i = 0; i < 2; i++)
            {
                if (player[i].action - 1 != field[i])
                {
                    if(spflag[i] == 0)
                    {
                        handimg[3 * i + field[i]].SetActive(false);
                        field[i] = player[i].action - 1;
                        handimg[3 * i + field[i]].SetActive(true);
                        atflag[i] = 0;
                        message.GetComponent<Text>().text = player[i].name + "は" + player[i].hands[field[i]].name + "を繰り出した！";
                        audiosource.PlayOneShot(appear);
                        yield return new WaitForSeconds(2.0f);
                    }
                    //バトンタッチ判定
                    else if (player[i].hands[field[i]].sp == 7 && player[i].hands[(field[i] + 2) % 4].hp > 0)
                    {
                        player[i].hands[field[i]].sp = 0;
                        handimg[3 * i + field[i]].SetActive(false);
                        field[i] = (field[i] + 2) % 4;
                        handimg[3 * i + field[i]].SetActive(true);
                        message.GetComponent<Text>().text = player[i].name + "はバトンタッチを使った！";
                        yield return Usesp(spimg[7]);
                        audiosource.PlayOneShot(appear);
                        yield return new WaitForSeconds(2.0f);
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                if (player[i].hands[field[i]].sp == 0) blindsp[i].SetActive(true);
                else blindsp[i].SetActive(false);
            }



            //相性確認
            eff = (field[0] - field[1] + 3) % 3;
            //あべこべ判定
            for(int i = 0; i < 2; i++)
            {
                if(spflag[i] == 1 && player[i].hands[field[i]].sp == 6)
                {
                    eff = (field[1] - field[0] + 3) % 3;
                    player[i].hands[field[i]].sp = 0;
                    message.GetComponent<Text>().text = player[i].name + "はあべこべを発動した！";
                    audiosource.PlayOneShot(genesp);
                    yield return Usesp(spimg[6]);
                    yield return new WaitForSeconds(2.0f);
                }
            }
            //痛み分け判定
            for (int i = 0; i < 2; i++)
            {
                if (spflag[i] == 1 && player[i].hands[field[i]].sp == 5)
                {
                    eff = 0;
                    player[i].hands[field[i]].sp = 0;
                    message.GetComponent<Text>().text = player[i].name + "はいたみわけを発動した！";
                    audiosource.PlayOneShot(genesp);
                    yield return Usesp(spimg[5]);
                    yield return new WaitForSeconds(2.0f);
                }
            }



            //攻撃
            rnd = 0;
            for(int i = 0; i < 2; i++)
            {

            }
            //Red勝ち
            if(eff == 2)
            {
                Attack(0, 1, 2);
                yield return new WaitForSeconds(waitS);
                Attack(1, 0, 1);
                yield return new WaitForSeconds(waitS);
            }
            //Green勝ち
            else if(eff == 1)
            {
                Attack(1, 0, 2);
                yield return new WaitForSeconds(waitS);
                Attack(0, 1, 1);
                yield return new WaitForSeconds(waitS);
            }
            //あいこ
            else
            {
                rnd = UnityEngine.Random.Range(0, 2);
                if(rnd == 1)
                {
                    Attack(0, 1, 0);
                    yield return new WaitForSeconds(waitS);
                    Attack(1, 0, 0);
                    yield return new WaitForSeconds(waitS);
                }
                else
                {
                    Attack(1, 0, 0);
                    yield return new WaitForSeconds(waitS);
                    Attack(0, 1, 0);
                    yield return new WaitForSeconds(waitS);
                }
            }
            

            //死亡判定
            for(int i = 0; i < 2; i++)
            {
                if(player[i].hands[field[i]].hp <= 0)
                {
                    player[i].handspoint -= 1;
                    handimg[3 * i + field[i]].SetActive(false);
                    fieldsta[i].SetActive(false);
                    message.GetComponent<Text>().text = player[i].name + "の" + player[i].hands[field[i]].name + "は力尽きてしまった・・・";
                    audiosource.PlayOneShot(down);
                    yield return new WaitForSeconds(2.0f);
                }
            }

            //終了判定
            if (FinishCheck() == 1) break;

            for (int i = 0; i < 2; i++)
            {
                //みちづれ判定
                if (player[i].hands[field[i]].hp <= 0)
                {
                    if(spflag[i] == 1 && player[i].hands[field[i]].sp == 4)
                    {
                        player[i].hands[field[i]].sp = 0;
                        message.GetComponent<Text>().text = player[i].name + "の" + player[i].hands[field[i]].name + "は相手を道連れにした！";
                        yield return Usesp(spimg[4]);
                        yield return new WaitForSeconds(2.0f);
                        handimg[3 * ((i + 1) % 2) + field[(i + 1) % 2]].SetActive(false);
                        fieldsta[(i + 1) % 2].SetActive(false);
                        audiosource.PlayOneShot(down);
                        message.GetComponent<Text>().text = player[(i + 1) % 2].name + "の" + player[(i + 1) % 2].hands[field[(i + 1) % 2]].name + "は力尽きてしまった・・・";
                        player[(i + 1) % 2].hands[field[(i + 1) % 2]].hp = 0;
                        player[(i + 1) % 2].handspoint -= 1;
                        yield return new WaitForSeconds(2.0f);
                    }
                }

                //ハンドクリーム判定
                else if(player[i].hands[field[i]].sp == 1 && player[i].hands[field[i]].hp <= 50)
                {
                    player[i].hands[field[i]].sp = 0;
                    message.GetComponent<Text>().text = player[i].name + "はハンドクリームを使用した！";
                    yield return Usesp(spimg[1]);
                    yield return new WaitForSeconds(2.0f);
                    player[i].hands[field[i]].hp += 30;
                    message.GetComponent<Text>().text = player[i].name + "の" + player[i].hands[field[i]].name + "は30回復した！";
                    audiosource.PlayOneShot(heal);
                    yield return new WaitForSeconds(2.0f);
                }   
            }

            //終了判定
            if (FinishCheck() == 1) break;

            //次手選択
            for (int i = 0; i < 2; i++)
            {
                if(player[i].hands[field[i]].hp <= 0)
                {
                    while (true)
                    {
                        message.GetComponent<Text>().text = player[i].name + "のターン：出す手を選択してください";
                        yield return new WaitUntil(() => Input.anyKeyDown);
                        field[i] = Func.GetCommand() - 1;
                        if (field[i] < 3)
                        {
                            if(player[i].hands[field[i]].hp > 0)
                            {
                                message.GetComponent<Text>().text = "OK";
                                audiosource.PlayOneShot(select);
                                yield return new WaitForSeconds(1.0f);
                                break;
                            }
                            else
                            {
                                message.GetComponent<Text>().text = "その手は選択できません";
                                yield return new WaitForSeconds(2.0f);
                            }
                        }
                        else if(field[i] == 7)
                        {
                            yield return null;
                        }
                        else
                        {
                            message.GetComponent<Text>().text = "未対応の入力です";
                            yield return new WaitForSeconds(2.0f);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < 2; i++)
        {
            if (player[i].hands[field[i]].sp == 0) blindsp[i].SetActive(true);
            else blindsp[i].SetActive(false);
        }

        //バトル終了
        yield return new WaitForSeconds(1.0f);
        Func.ChangeScene("ResultScene");
    }





    
    void Attack(int attacker, int target, int effective)
    {
        int damage = 0;
        waitS = 0;
        if(player[attacker].hands[field[attacker]].hp > 0 && atflag[attacker] == 1)
        {
            int rnd = UnityEngine.Random.Range(0, 10);
            string spstring = null;
            //ハンドガン判定
            if(spflag[attacker] == 1 && player[attacker].hands[field[attacker]].sp == 3)
            {
                player[attacker].hands[field[attacker]].sp = 0;
                atflag[attacker] = 0;
                spstring = "ハンドガンを使った！！！\r\n";
                StartCoroutine(Usesp(spimg[3]));
                audiosource.PlayOneShot(handgun);
                waitS += 1;
                if (rnd > 7)
                {
                    damage = 999;
                }
            }

            //攻撃判定
            if(atflag[attacker] == 1)
            {
                //あいこ
                if(effective == 0)
                {
                    damage = 30;
                }
                //負け
                else if(effective == 1)
                {
                    damage = 20;
                }
                //勝ち
                else
                {
                    damage = 50;
                }
            }

            //ラッキーパンチ判定
            if(player[attacker].hands[field[attacker]].sp == 2　&& rnd > 8)
            {
                damage *= 2;
                waitS += 1;
                spstring = "ラッキーパンチ！！！\r\n";
                StartCoroutine(Usesp(spimg[2]));
            }

            //ダメージ処理
            if (player[target].hands[field[target]].hp >= damage)
            {
                player[target].hands[field[target]].hp -= damage;
                audiosource.PlayOneShot(attack);
            }
            else
            {
                player[target].hands[field[target]].hp = 0;
                audiosource.PlayOneShot(attack);
            }
            if(damage > 0)
            {
                StartCoroutine(Blink(handimg[3 * target + field[target]]));
            }
            message.GetComponent<Text>().text = $"{player[attacker].name}の攻撃：{spstring}相手に{damage}のダメージ！";
            waitS += 2.5f;
        }
    }





    IEnumerator Blink(GameObject img)
    {
        yield return null;
        img.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        img.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        img.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        img.SetActive(true);
    }

    IEnumerator Usesp(GameObject img)
    {
        yield return null;
        img.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        img.SetActive(false);
    }






    public int FinishCheck()
    {
        for(int i = 0; i < 2; i++)
        {
            if(player[i].handspoint == 0)
            {
                winner = player[(i + 1) % 2].name;
                return 1;
            }
        }
        return 0;
    }




    void Update()
    {
        //ステータス表示
        for(int i = 0; i < 2; i++)
        {
            if(field[i] < 3)
            {
                fieldsta[i].GetComponent<Text>().text = $"{player[i].hands[field[i]].name}\r\nHP{player[i].hands[field[i]].hp}";
                for (int j = 0; j < 3; j++)
                {
                    handsta[3 * i + j].GetComponent<Text>().text = (j + 1).ToString("D") + "." + player[i].hands[j].name + "：HP" + player[i].hands[j].hp.ToString("D");
                }
            }
            
        }

        //スペシャルリスト表示
        if(Input.GetKeyDown(KeyCode.KeypadEnter) || (Input.GetKeyDown(KeyCode.Return)))
        {
            liston = !liston;
            splist[0].SetActive(liston);
            splist[1].SetActive(liston);
            splist[2].SetActive(liston);
        }

        //タイム管理
        if (timeron)
        {
            time -= Time.deltaTime;
            timer.SetActive(true);
            timer.GetComponent<Text>().text = ($"残り {(int)time} 秒");
        }
    }
    
}
