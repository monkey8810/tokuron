using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tokuron
{
    public static class Problem0
    {
        static int INIT_STATE = 0;//初期状態
        static int state = INIT_STATE;//現状態を保存する変数（念のため初期状態で初期化）
        static int seed = Environment.TickCount;

        //報酬関数(アクション3の行動を取った時のみ報酬10)
        static double[] reward = { -1, -1, -1, 10 };
        //勝利点をカウント(4で終了)
        static int count_tokuten = 0;
        //デッキを初期化(銅*7,お邪魔*3)
        static int[] deck = new int[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 };

        //現状態を返す
        public static int getState()
        {
            return state;
        }

        //エピソード開始時に呼び出すメソッド
        //状態(デッキ)を初期化する
        public static void initState()
        {
            count_tokuten = 0;
            //setState(INIT_STATE);
            deck = new int[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1 };
            deck = deck.OrderBy(i => Guid.NewGuid()).ToArray();
            setState(deck[0] + deck[1] + deck[2] + deck[3] + deck[4]);
        }

        //エージェントが現状態が終状態かどうか知りたいときに呼び出すメソッド
        //現状態が終状態かどうかを返す
        public static bool getStateIsGoal()
        {
            bool isGoal = false;
            //勝利点が4枚なら終状態
            if (count_tokuten == 4) isGoal = true;
            return isGoal;
        }

        //状態を任意の状態へ変更（普通は使わない）
        private static void setState(int s)
        {
            state = s;
        }

        //現状態の報酬を返す（普段は使わない）
        private static double getReward(int act)
        {
            return reward[act];
        }

        //行動決定()
        public static int step(int s, double[,] Q)
        {
            Random cRandom = new System.Random(seed++);
            int action = 0;
            int[] index = new int[4];
            int epsilon, y;
            epsilon = 30;
            y = cRandom.Next(0, 100);
            double[] max = new double[4];

            if (epsilon < y)
            {
                //Console.WriteLine("g");
                //現状態でQテーブルの1, 2, 3番目を求める
                for (int i = 0; i < 4; i++)
                {
                    if (max[0] < Q[s, i])
                    {
                        max[3] = max[2];
                        max[2] = max[1];
                        max[1] = max[0];
                        max[0] = Q[s, i];

                        index[3] = index[2];
                        index[2] = index[1];
                        index[1] = index[0];
                        index[0] = i;
                    }
                    else if (max[1] < Q[s, i])
                    {
                        max[3] = max[2];
                        max[2] = max[1];
                        max[1] = Q[s, i];

                        index[3] = index[2];
                        index[2] = index[1];
                        index[1] = i;
                    }
                    else if (max[2] < Q[s, i])
                    {
                        max[3] = max[2];
                        max[2] = Q[s, i];

                        index[3] = index[2];
                        index[2] = i;
                    }
                    else if (max[3] < Q[s, i])
                    {
                        max[3] = Q[s, i];

                        index[3] = i;
                    }
                }
                if (s == 2)
                {
                    action = 0;
                    return action;
                }
                action = index[0];

                //行動を制限(次にQ値の良い行動をとる)
                for (int i = 1; i < 4; i++)
                {
                    if (s == 3 || s == 4 || s == 5)
                    {
                        if (action == 2 || action == 3)
                        {
                            action = index[i];
                        }
                        else
                        {
                            return action;
                        }
                    }
                    if (s == 6 || s == 7)
                    {
                        if (action == 3)
                        {
                            action = index[i];
                        }
                        else
                        {
                            return action;
                        }
                    }
                }
                return action;
            }
            else
            {
                //Console.WriteLine("r");
                if (s == 2)
                {
                    action = 0;
                    return action;
                }
                else if (s == 3 || s == 4 || s == 5)
                {
                    action = cRandom.Next(2);
                    return action;
                }
                else if (s == 6 || s == 7)
                {
                    action = cRandom.Next(3);
                    return action;
                }
                else
                {
                    action = cRandom.Next(4);
                    return action;
                }
            }
        }

        //エージェントが環境上で行動を実行するときに呼び出すメソッド
        //行動に対応する状態遷移を実行＆報酬値を返す
        public static double doAction(int act)
        {
            /*
            // デッキ確認用
            foreach (int i in deck)
            {
                Console.Write("{0} ", i);
            }
            Console.WriteLine();
            */

            //actが0なら何もしない、1なら銀貨、2なら金貨、3なら勝利点を購入
            if (act == 0)
            {

            }
            else if (act == 1)
            {
                deck.CopyTo(deck = new int[deck.Length + 1], 0);
                deck[deck.Length - 1] = 2;
            }
            else if (act == 2)
            {
                deck.CopyTo(deck = new int[deck.Length + 1], 0);
                deck[deck.Length - 1] = 3;
            }
            else if (act == 3)
            {
                deck.CopyTo(deck = new int[deck.Length + 1], 0);
                deck[deck.Length - 1] = 0;
                count_tokuten++;
            }
            deck = deck.OrderBy(i => Guid.NewGuid()).ToArray();
            setState(deck[0] + deck[1] + deck[2] + deck[3] + deck[4]);
            return (double)getReward(act);
        }
    }
}
