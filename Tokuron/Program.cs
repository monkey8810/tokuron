using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tokuron
{
    class Program
    {
        static void Main(string[] args)
        {
            /*** 適宜，必要な初期化処理 ***/
            int s;//状態を保存する変数
            int s_pre;//前状態を保存する変数
            int a;//行動を保存する変数
            double gamma = 0.95, arufa = 0.1;
            double[,] Q = new double[16, 4];//状態行動価値を保存する変数
            double r;//報酬を保存する変数
            const int MAX_CYCLE = 10000;

            //Cycleの進行
            for (int cycle = 0; cycle < MAX_CYCLE; cycle++)
            {
                int turn = 0;
                //Console.WriteLine((cycle + 1) + "回目");
                Problem0.initState();
                s = Problem0.getState();
                //Console.WriteLine("s = " + s);

                //Episodeの進行
                while (true)
                {

                    /*for (int i = 2; i < 16; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Console.Write(Q[i, j] + " ");
                        }
                        Console.WriteLine("");
                    }*/
                    //行動決定
                    a = Problem0.step(s, Q);
                    //Console.WriteLine(a + "を実行");

                    s_pre = Problem0.getState();

                    //行動aの実行＆報酬rの観測＆状態遷移
                    r = Problem0.doAction(a);
                    turn++;
                    double max = Q[s, 0];
                    for (int i = 0; i < 4; i++)
                    {
                        if (max < Q[s, i])
                        {
                            max = Q[s, i];
                        }
                    }
                    //ゴール判定
                    if (Problem0.getStateIsGoal())
                    {
                        Q[s_pre, a] = Q[s_pre, a] + arufa * (r + gamma * 0 - Q[s_pre, a]);
                        if (cycle % 100 == 0)
                        {
                            Console.WriteLine(turn + "ターンで終了状態に到達");
                            //Qテーブル確認
                            for (int i = 2; i < 16; i++)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    Console.Write(Q[i, j] + " ");
                                }
                                Console.WriteLine("");
                            }
                            Console.WriteLine("");
                        }
                        //Console.WriteLine("");
                        break;
                    }
                    Q[s_pre, a] = Q[s_pre, a] + arufa * (r + gamma * max - Q[s_pre, a]);
                    //Console.WriteLine(r + "を獲得（報酬）");
                    //Console.WriteLine("s'=" + Problem0.getState());

                    //次のEpisodeに備えてs := s'
                    s = Problem0.getState();
                    //Console.WriteLine("s=" + s_pre);
                    //Console.WriteLine();
                }
            }
            Console.In.ReadLine();
        }
    }
}
