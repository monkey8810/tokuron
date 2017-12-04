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
            int[] turn = new int[1000000];
            /*** 適宜，必要な初期化処理 ***/
            int s;//状態を保存する変数
            int s_pre;//前状態を保存する変数
            int a;//行動を保存する変数
            double gamma = 0.95, alpha = 0.1;
            double[,] Q = new double[16, 4];//状態行動価値を保存する変数
            double r; //報酬を保存する変数
            const int MAX_CYCLE = 1000000;    //学習回数

            //Cycleの進行
            for (int cycle = 0; cycle < MAX_CYCLE; cycle++)
            {
                //Console.WriteLine((cycle + 1) + "回目");
                Problem0.initState();
                s = Problem0.getState();
                //Console.WriteLine("s = " + s);

                //Episodeの進行
                while (true)
                {
                    /*
                    //Q_tableの確認
                    for (int i = 2; i < 16; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Console.Write(Q[i, j] + " ");
                        }
                        Console.WriteLine("");
                    }
                    */
                    //行動決定
                    a = Problem0.step(s, Q);
                    //Console.WriteLine(a + "を実行");
                    //行動前に状態を保存
                    s_pre = Problem0.getState();
                    //行動aの実行&報酬rの取得
                    r = Problem0.doAction(a);
                    //行動後の状態に更新
                    s = Problem0.getState();
                    //次状態でQ値の最大値を取得
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
                        //Q_tableの更新(最後のみ次状態なし)
                        Q[s_pre, a] = Q[s_pre, a] + alpha * (r + gamma * 0 - Q[s_pre, a]);
                        if (cycle % 100000 == 0)
                        {
                            Console.WriteLine(cycle + "回目:" + turn[cycle] + "ターンで終了状態に到達");
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
                    //Q_tableの更新
                    Q[s_pre, a] = Q[s_pre, a] + alpha * (r + gamma * max - Q[s_pre, a]);

                    //Console.WriteLine(r + "を獲得（報酬）");
                    //Console.WriteLine("s(前状態)=" + s_pre);
                    //Console.WriteLine("s'(次状態)=" + s);
                    //Console.WriteLine();
                    turn[cycle]++;
                }
            }
            try
            {
                // appendをtrueにすると，既存のファイルに追記
                //         falseにすると，ファイルを新規作成する
                var append = false;
                // 出力用のファイルを開く
                using (var sw = new System.IO.StreamWriter(@"average.csv", append))
                {
                    for (int i = 0; i < 1000000; i = i + 10000)
                    {
                        int sum = 0;
                        for (int j = 0; j < 10000; j++)
                        {
                            sum += turn[i + j];
                        }
                        int heikin = 0;
                        heikin = sum / 10000;
                        sw.WriteLine("{0} ~ {1}, {2},",i + 1, i + 9999, heikin);
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したときエラーメッセージを表示
                System.Console.WriteLine(e.Message);
            }
            /*for (int i = 0; i < 10000; i = i + 100)
            {
                int sum = 0;
                for (int j = 0; j < 100; j++)
                {
                    sum += turn[i + j];
                }
                int heikin = 0;
                heikin = sum / 100;
                Console.WriteLine(heikin);
            }
            */
            Console.WriteLine("-----------------------------");
            try
            {
                // appendをtrueにすると，既存のファイルに追記
                //         falseにすると，ファイルを新規作成する
                var append = false;
                // 出力用のファイルを開く
                using (var sw = new System.IO.StreamWriter(@"test.csv", append))
                {
                    for (int i = 0; i < turn.Length; ++i)
                    {
                        // 
                        sw.WriteLine("{0}", turn[i]);
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したときエラーメッセージを表示
                System.Console.WriteLine(e.Message);
            }
            
            Console.In.ReadLine();
        }
    }
}
