using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace jp.ktsystem.kadai201411.h_okita
{
    /// <summary>
    /// 2014/11勉強会課題クラス
    /// </summary>
    public class Kadai
    {
        /// <summary>文字エンコード </summary>
        private static readonly Encoding ENCODING = Encoding.UTF8;
        /// <summary>Lv1出力ファイル名</summary>
        private static readonly string OUTPUT_FILE_NAME = "ordercount.out";
        /// <summary>入金ファイル名</summary>
        private static readonly string INCOME_FILE_NAME = "income.txt";
        /// <summary>生産指示ファイル名</summary>
        private static readonly string PRODUCT_ORDER_FILE_NAME = "productorder.out";
        /// <summary>退避ファイル出力フォルダ名</summary>
        private static readonly string RESERVATION_FOLDER_NAME = "Reservation";
        /// <summary>退避ファイル名</summary>
        private static readonly string RESERVATION_FILE_NAME = "reservation.dat";

        /// <summary>受注情報ファイルの受注IDインデックス</summary>
        private static readonly int ID_INDEX_OF_ORDER_FILE = 0;
        /// <summary>受注情報ファイルの顧客名インデックス</summary>
        private static readonly int CYSTOMER_NAME_INDEX_OF_ORDER_FILE = 1;
        /// <summary>受注情報ファイルの製品名インデックス</summary>
        private static readonly int PRODUCT_NAME_INDEX_OF_ORDER_FILE = 2;
        /// <summary>受注情報ファイルの数量インデックス</summary>
        private static readonly int QUANTITY_INDEX_OF_ORDER_FILE = 3;
        /// <summary>受注情報ファイルの納期インデックス</summary>
        private static readonly int DELIVERY_DATE_OF_ORDER_FILE = 4;

        /// <summary>入金情報ファイルの受注IDインデックス</summary>
        private static readonly int ID_INDEX_OF_INCOME_FILE = 0;
        /// <summary>入金情報ファイルの入金日時インデックス</summary>
        private static readonly int DESPOSIT_DATE_INDEX_OF_INCOME_FILE = 1;

        /// <summary>受注情報ファイルの納期フォーマット</summary>
        private static readonly string DELIVERY_OF_ORDER_FORMAT = "yyyyMMdd";
        /// <summary>受注情報ファイルの正規表現納期フォーマット</summary>
        private static readonly string DELIVERY_OF_ORDER_REG_FORMAT = @"^(?<year>[0-9]{4})(?<month>0?[1-9]|1[012])(?<day>0?[1-9]|[12][0-9]|3[01])$";
        /// <summary>入金情報ファイルの入金日時フォーマット</summary>
        private static readonly string DESPOSIT_OF_INCOME_FORMAT = "yyyyMMddHHmmss";
        /// <summary>入金情報ファイルの正規表現入金日時フォーマット</summary>
        private static readonly string DESPOSIT_OF_INCOME_REG_FORMAT = @"^(?<year>[0-9]{4})(?<month>0?[1-9]|1[012])(?<day>0?[1-9]|[12][0-9]|3[01])(?<hour>0?[1-9]|1[0-9]|2[0-3])(?<minute>0?[1-9]|[1-5][0-9])(?<second>0?[1-9]|[1-5][0-9])$";

        /// <summary>
        /// 問１ 受注情報に存在する製品と受注数量の一覧を出力する
        /// </summary>
        /// <param name="anOrderFileDir">受注情報ファイル入力ディレクトリ</param>
        /// <param name="anOutputDir">出力ディレクトリ</param>
        /// <returns>出力したレコードの件数（異常終了の場合はエラーコード）</returns>
        public static int CountOrder(string anOrderFileDir, string anOutputDir)
        {
            //受注情報ファイル入力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anOrderFileDir) || !Directory.Exists(@anOrderFileDir))
            {
                return Constants.INPUT_ERRER_OF_ORDER_FILE;
            }

            //出力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anOutputDir) || !Directory.Exists(@anOutputDir))
            {
                return Constants.OUTPUT_ERRER_OF_FIRST_QUESTION;
            }

            //入力フォルダ内のファイル一覧取得
            List<string> inputFiles = Directory.GetFiles(@anOrderFileDir, "*", System.IO.SearchOption.TopDirectoryOnly).ToList();
            //ファイル名のソート
            inputFiles.Sort(StringComparer.InvariantCulture);

            //受注情報のリストを取得
            int errerNo = 0;    //エラー番号
            List<OrderFileDataMolde> orderDataList = GetOrderFileData(inputFiles.ToList(), out errerNo);
            if (null == orderDataList)
            {
                return errerNo;
            }

            //受注情報データから、製品名ごとに数量を集計する
            Dictionary<string, int> outPutData = GetTotalProductionsQuantity(orderDataList);

            return OutPutOrderCount(outPutData, anOutputDir + "/" + OUTPUT_FILE_NAME);
        }

        /// <summary>
        /// 問２ 受注情報と入金情報を元に生産指示情報を出力する
        /// </summary>
        /// <param name="anOrderFileDir">受注情報ファイル入力ディレクトリ</param>
        /// <param name="anIncomeFileDir">入金情報ファイル入力ディレクトリ</param>
        /// <param name="anOutputDir">出力ディレクトリ</param>
        /// <param name="aBackupDir">バックアップファイル出力ディレクトリ</param>
        /// <returns>出力したレコードの件数（異常終了の場合はエラーコード）</returns>
        public static int createProductOrder(String anOrderFileDir, String anIncomeFileDir, String anOutputDir, String aBackupDir)
        {
            //受注情報ファイル入力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anOrderFileDir) || !Directory.Exists(@anOrderFileDir))
            {
                return Constants.INPUT_ERRER_OF_ORDER_FILE;
            }

            //入金情報ファイル出力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anIncomeFileDir) || !Directory.Exists(@anIncomeFileDir))
            {
                return Constants.INPUT_ERRER_OF_INCOME_FILE;
            }

            //出力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anOutputDir) || !Directory.Exists(@anOutputDir))
            {
                return Constants.OUTPUT_ERRER_OF_PRODUCT_ORDER_FILE;
            }

            //バックアップファイル出力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(aBackupDir) || !Directory.Exists(@aBackupDir))
            {
                return Constants.ERRER_OF_BACKUP;
            }

            //受注情報フォルダ内の受注情報ファイル一覧取得
            List<string> inputFiles = Directory.GetFiles(@anOrderFileDir, "*", System.IO.SearchOption.TopDirectoryOnly).ToList();
            //受注情報ファイル名のソート
            inputFiles.Sort(StringComparer.InvariantCulture);

            //退避ファイルパス取得
            string reservationFileDir = GetReservationFileDir() + "/" + RESERVATION_FILE_NAME;
            bool existsReservation = false; //入力退避ファイル存在フラグ
            if (!string.IsNullOrEmpty(@reservationFileDir) && File.Exists(@reservationFileDir))
            {
                //退避ファイルパスを受注情報ファイル一覧の先頭に追加
                inputFiles.Insert(0, @reservationFileDir);
                existsReservation = true;
            }

            //受注情報のリストを取得
            int errerNo = 0;    //エラー番号
            List<OrderFileDataMolde> orderDataList = GetOrderFileData(inputFiles.ToList(), existsReservation, out errerNo);
            if (null == orderDataList)
            {
                return errerNo;
            }

            //入金情報のリストを取得
            List<IncomeFileDataModel> incomeDataList = GetIncomeFileData(anIncomeFileDir + "/" + INCOME_FILE_NAME, out errerNo);
            if (null == incomeDataList)
            {
                return errerNo;
            }

            //生産指示情報及び、生産指示ファイルに出力しなかった受注情報のデータを取得する
            ProductAndOrderDataModel resultData = GetProductAndOrderData(orderDataList, incomeDataList);

            //生産指示出力
            int result = OutPutProductOrderFile(resultData.OutPutProductionDataList, @anOutputDir + "/" + PRODUCT_ORDER_FILE_NAME);
            if (Constants.SUCCESS != result)
            {
                return result;
            }

            //退避ディレクトリが存在しない場合、フォルダ作成
            if (0 < resultData.OutPutOrderDataList.Count)
            {
                string reservationDir = GetReservationFileDir();
                if (!Directory.Exists(reservationDir))
                {
                    Directory.CreateDirectory(@reservationDir);
                }
                //生産指示ファイルに出力しなかった受注情報のデータを出力
                string reservationFilePath = GetReservationFileDir() + "/" + RESERVATION_FILE_NAME;
                result = OutPutOrderFile(resultData.OutPutOrderDataList, reservationFilePath);
                if (Constants.SUCCESS != result)
                {
                    return result;
                }
            }

            //使用した受注情報ファイルをバックアップフォルダへ移動
            if (existsReservation)
            {
                inputFiles.RemoveAt(0);
            }
            result = MoveFiles(inputFiles, aBackupDir);
            if (Constants.SUCCESS != result)
            {
                return result;
            }

            //出力した生産指示レコード数を返す
            return resultData.OutPutProductionDataList.Count;
        }

        /// <summary>
        /// 文字列が半角数字のみかどうか調べる
        /// </summary>
        /// <param name="str">調べる文字列</param>
        /// <returns>半角数字：true、それ以外：false</returns>
        public static bool IsHalfNumber(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]+$");
        }

        /// <summary>
        /// 退避ファイル出力ディレクトリ取得
        /// </summary>
        /// <returns>退避ファイル出力ディレクトリ</returns>
        public static string GetReservationFileDir()
        {
            //現在作業ディレクトリ
            string currentPath = System.IO.Directory.GetCurrentDirectory();
            return currentPath + "/" + RESERVATION_FOLDER_NAME;
        }

        /// <summary>
        /// 指定された受注情報ファイルから、受注情報のリストを取得する
        /// </summary>
        /// <param name="files">受注情報ファイルパスのリスト</param>
        /// <param name="errerNo">エラー番号</param>
        /// <returns>受注情報のリスト（エラーが発生し処理が中断した場合は、nullが返る）</returns>
        private static List<OrderFileDataMolde> GetOrderFileData(List<string> files, out int errerNo)
        {
            //受注情報のリスト
            List<OrderFileDataMolde> orderDataList = new List<OrderFileDataMolde>();

            try
            {
                //ファイル読み込み
                foreach (string filePath in files)
                {
                    using (StreamReader sr = new StreamReader(@filePath, ENCODING))
                    {
                        //ストリームの末尾まで繰り返す
                        while (!sr.EndOfStream)
                        {
                            //ファイルから一行読み込む
                            string line = sr.ReadLine();
                            //読み込んだ一行をカンマ毎に分けて配列に格納する
                            string[] values = line.Split(',');

                            //フィールド数のチェック
                            if (5 != values.Length)
                            {
                                errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                return null;
                            }

                            //必須フィールドの入力チェック
                            if (string.IsNullOrEmpty(values[ID_INDEX_OF_ORDER_FILE]) || string.IsNullOrEmpty(values[CYSTOMER_NAME_INDEX_OF_ORDER_FILE]) ||
                                string.IsNullOrEmpty(values[PRODUCT_NAME_INDEX_OF_ORDER_FILE]) || string.IsNullOrEmpty(values[QUANTITY_INDEX_OF_ORDER_FILE]))
                            {
                                errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                return null;
                            }

                            //数量フィールドが半角数字のみで入力されているかのチェック
                            if (!IsHalfNumber(values[QUANTITY_INDEX_OF_ORDER_FILE]))
                            {
                                errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                return null;
                            }

                            //納期のチェック
                            if (string.Empty != values[DELIVERY_DATE_OF_ORDER_FILE])
                            {
                                //納期の日付フォーマットチェック
                                if (!Regex.IsMatch(values[DELIVERY_DATE_OF_ORDER_FILE], DELIVERY_OF_ORDER_REG_FORMAT))
                                {
                                    errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                    return null;
                                }
                                //納期の日付チェック
                                DateTime date;
                                if (!DateTime.TryParseExact(values[DELIVERY_DATE_OF_ORDER_FILE], DELIVERY_OF_ORDER_FORMAT,
                                    System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out date))
                                {
                                    errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                    return null;
                                }
                            }

                            //受注情報モデルの作成
                            OrderFileDataMolde dataModel = new OrderFileDataMolde(values[ID_INDEX_OF_ORDER_FILE], values[CYSTOMER_NAME_INDEX_OF_ORDER_FILE], values[PRODUCT_NAME_INDEX_OF_ORDER_FILE],
                                int.Parse(values[QUANTITY_INDEX_OF_ORDER_FILE]), values[DELIVERY_DATE_OF_ORDER_FILE]);

                            int index = orderDataList.BinarySearch(dataModel);

                            if (0 > index)
                            {
                                //同一の受注IDが存在しない場合、リストの適切なインデックスに挿入
                                orderDataList.Insert(~index, dataModel);
                            }
                            else
                            {
                                //同一の受注IDが存在する場合、中身の置き換え
                                orderDataList[index].CustomerName = dataModel.CustomerName;
                                orderDataList[index].ProductionName = dataModel.ProductionName;
                                orderDataList[index].Quantity = dataModel.Quantity;
                                orderDataList[index].DateOfDelivery = dataModel.DateOfDelivery;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                errerNo = Constants.INPUT_ERRER_OF_ORDER_FILE;
                return null;
            }

            errerNo = Constants.SUCCESS;
            return orderDataList;
        }

        /// <summary>
        /// 指定された受注情報ファイルから、受注情報のリストを取得する
        /// </summary>
        /// <param name="files">受注情報ファイルパスのリスト</param>
        /// <param name="existsReservation">退避ファイル存在フラグ</param>
        /// <param name="errerNo">エラー番号</param>
        /// <returns>受注情報のリスト（エラーが発生し処理が中断した場合は、nullが返る）</returns>
        private static List<OrderFileDataMolde> GetOrderFileData(List<string> files, bool existsReservation, out int errerNo)
        {
            //受注情報のリスト
            List<OrderFileDataMolde> orderDataList = new List<OrderFileDataMolde>();

            int i = 0;
            try
            {
                //ファイル読み込み
                for (i = 0; i < files.Count; i++)
                {
                    using (StreamReader sr = new StreamReader(@files[i], ENCODING))
                    {
                        //ストリームの末尾まで繰り返す
                        while (!sr.EndOfStream)
                        {
                            //ファイルから一行読み込む
                            string line = sr.ReadLine();
                            //読み込んだ一行をカンマ毎に分けて配列に格納する
                            string[] values = line.Split(',');

                            //フィールド数のチェック
                            if (5 != values.Length)
                            {
                                errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                return null;
                            }

                            //必須フィールドの入力チェック
                            if (string.IsNullOrEmpty(values[ID_INDEX_OF_ORDER_FILE]) || string.IsNullOrEmpty(values[CYSTOMER_NAME_INDEX_OF_ORDER_FILE]) ||
                                string.IsNullOrEmpty(values[PRODUCT_NAME_INDEX_OF_ORDER_FILE]) || string.IsNullOrEmpty(values[QUANTITY_INDEX_OF_ORDER_FILE]))
                            {
                                errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                return null;
                            }

                            //数量フィールドが半角数字のみで入力されているかのチェック
                            if (!IsHalfNumber(values[QUANTITY_INDEX_OF_ORDER_FILE]))
                            {
                                errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                return null;
                            }

                            //納期のチェック
                            if (string.Empty != values[DELIVERY_DATE_OF_ORDER_FILE])
                            {
                                //納期の日付フォーマットチェック
                                if (!Regex.IsMatch(values[DELIVERY_DATE_OF_ORDER_FILE], DELIVERY_OF_ORDER_REG_FORMAT))
                                {
                                    errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                    return null;
                                }
                                //納期の日付チェック
                                DateTime date;
                                if (!DateTime.TryParseExact(values[DELIVERY_DATE_OF_ORDER_FILE], DELIVERY_OF_ORDER_FORMAT,
                                    System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out date))
                                {
                                    errerNo = Constants.FORMAT_ERRER_OF_ORDER_FILE;
                                    return null;
                                }
                            }

                            //受注情報モデルの作成
                            OrderFileDataMolde dataModel = new OrderFileDataMolde(values[ID_INDEX_OF_ORDER_FILE], values[CYSTOMER_NAME_INDEX_OF_ORDER_FILE], values[PRODUCT_NAME_INDEX_OF_ORDER_FILE],
                                int.Parse(values[QUANTITY_INDEX_OF_ORDER_FILE]), values[DELIVERY_DATE_OF_ORDER_FILE]);

                            int index = orderDataList.BinarySearch(dataModel);

                            if (0 > index)
                            {
                                //同一の受注IDが存在しない場合、リストの適切なインデックスに挿入
                                orderDataList.Insert(~index, dataModel);
                            }
                            else
                            {
                                //同一の受注IDが存在する場合、中身の置き換え
                                orderDataList[index].CustomerName = dataModel.CustomerName;
                                orderDataList[index].ProductionName = dataModel.ProductionName;
                                orderDataList[index].Quantity = dataModel.Quantity;
                                orderDataList[index].DateOfDelivery = dataModel.DateOfDelivery;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (existsReservation && 0 == i)
                {
                    errerNo = Constants.INPUT_ERRER_OF_RESERVATION_FILE;
                }
                else
                {
                    errerNo = Constants.INPUT_ERRER_OF_ORDER_FILE;
                }
                return null;
            }

            errerNo = Constants.SUCCESS;
            return orderDataList;
        }

        /// <summary>
        /// 受注情報データから、製品名ごとに数量を集計する
        /// </summary>
        /// <param name="orderDataList">受注情報のリスト</param>
        /// <param name="anOutputDir">出力ファイルパス</param>
        private static Dictionary<string, int> GetTotalProductionsQuantity(List<OrderFileDataMolde> orderDataList)
        {
            //集計後のデータ
            Dictionary<string, int> returnData = new Dictionary<string, int>();

            //同一製品名によるgroup by
            var query = from p in orderDataList orderby p.ProductionName group p.Quantity by p.ProductionName;

            foreach (var data in query)
            {
                returnData.Add(data.Key, data.Sum());
            }
            return returnData;
        }

        /// <summary>
        /// 製品と受注数量の一覧を上書き出力する
        /// </summary>
        /// <param name="outPutData">受注情報のリスト</param>
        /// <param name="anOutputPath">出力ファイルパス</param>
        /// <returns>出力したレコードの件数（異常終了の場合はエラーコード）</returns>
        private static int OutPutOrderCount(Dictionary<string, int> outPutData, string anOutputPath)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(@anOutputPath, false, ENCODING))
                {
                    foreach (string key in outPutData.Keys)
                    {
                        sr.WriteLine(string.Format("{0},{1}", key, outPutData[key]));
                    }
                }
            }
            catch (Exception)
            {
                return Constants.OUTPUT_ERRER_OF_FIRST_QUESTION;
            }
            return outPutData.Count;
        }

        /// <summary>
        /// 指定された入金情報ファイルから、入金情報のリストを取得する
        /// </summary>
        /// <param name="file">入金ファイルパス</param>
        /// <param name="errerNo">エラー番号</param>
        /// <returns>入金情報のリスト（エラーが発生し処理が中断した場合は、nullが返る）</returns>
        private static List<IncomeFileDataModel> GetIncomeFileData(string filePath, out int errerNo)
        {
            //入金情報のリスト
            List<IncomeFileDataModel> incomeDataList = new List<IncomeFileDataModel>();

            if (!File.Exists(@filePath))
            {
                errerNo = Constants.SUCCESS;
                return incomeDataList;
            }

            try
            {
                //ファイル読み込み
                using (StreamReader sr = new StreamReader(@filePath, ENCODING))
                {
                    //ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        //ファイルから一行読み込む
                        string line = sr.ReadLine();
                        //読み込んだ一行をカンマ毎に分けて配列に格納する
                        string[] values = line.Split(',');

                        //フィールド数のチェック
                        if (2 != values.Length)
                        {
                            errerNo = Constants.FORMAT_ERRER_OF_INCOME_FILE;
                            return null;
                        }

                        //必須フィールドの入力チェック
                        if (string.IsNullOrEmpty(values[ID_INDEX_OF_INCOME_FILE]) || string.IsNullOrEmpty(values[DESPOSIT_DATE_INDEX_OF_INCOME_FILE]))
                        {
                            errerNo = Constants.FORMAT_ERRER_OF_INCOME_FILE;
                            return null;
                        }

                        //入金日時のフォーマットチェック
                        if (!Regex.IsMatch(values[DESPOSIT_DATE_INDEX_OF_INCOME_FILE], DESPOSIT_OF_INCOME_REG_FORMAT))
                        {
                            errerNo = Constants.FORMAT_ERRER_OF_INCOME_FILE;
                            return null;
                        }
                        //入金日時のDateTime変換
                        DateTime despositDate;
                        if (!DateTime.TryParseExact(values[DESPOSIT_DATE_INDEX_OF_INCOME_FILE], DESPOSIT_OF_INCOME_FORMAT,
                            System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out despositDate))
                        {
                            errerNo = Constants.FORMAT_ERRER_OF_INCOME_FILE;
                            return null;
                        }

                        //入金情報モデルの作成
                        IncomeFileDataModel dataModel = new IncomeFileDataModel(values[ID_INDEX_OF_INCOME_FILE], despositDate);

                        int index = incomeDataList.BinarySearch(dataModel);

                        if (0 > index)
                        {
                            //同一の受注IDが存在しない場合、リストの適切なインデックスに挿入
                            incomeDataList.Insert(~index, dataModel);
                        }
                        else
                        {
                            //同一の受注IDが存在する場合、入金日時が早いほうで上書き
                            if (incomeDataList[index].DepositDate > dataModel.DepositDate)
                            {
                                incomeDataList[index].DepositDate = dataModel.DepositDate;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                errerNo = Constants.INPUT_ERRER_OF_INCOME_FILE;
                return null;
            }

            errerNo = Constants.SUCCESS;
            return incomeDataList;
        }

        /// <summary>
        /// 生産指示情報及び、生産指示ファイルに出力しなかった受注情報を取得する
        /// </summary>
        /// <param name="orderData">受注情報</param>
        /// <param name="incomeData">入金情報</param>
        /// <returns>生産指示情報及び、生産指示ファイルに出力しなかった受注情報</returns>
        private static ProductAndOrderDataModel GetProductAndOrderData(List<OrderFileDataMolde> orderDataList, List<IncomeFileDataModel> incomeDataList)
        {
            //受注情報に入金情報を左側外部結合したデータ（入金情報が存在しない場合、入金日時にはnullが入る）
            var leftOuterJoinQuery = from orderData in orderDataList
                                     join incomeData in incomeDataList on orderData.Id equals incomeData.Id into productionGrop
                                     from item in productionGrop.DefaultIfEmpty(new IncomeFileDataModel(null, null))
                                     select new
                                     {
                                         Id = orderData.Id,
                                         CustomerName = orderData.CustomerName,
                                         ProductName = orderData.ProductionName,
                                         Quantity = orderData.Quantity,
                                         DateOfDelivery = orderData.DateOfDelivery,
                                         DepositDate = item.DepositDate
                                     };

            //生産指示情報リスト作成（入金日時の昇順、IDの昇順で並び替え）
            var productionsData = from prodData in leftOuterJoinQuery
                                  where prodData.DepositDate != null
                                  orderby prodData.DepositDate, prodData.Id
                                  select prodData;
            List<ProductOrderFileDataModle> prodDataList = new List<ProductOrderFileDataModle>();
            foreach (var prod in productionsData)
            {
                prodDataList.Add(new ProductOrderFileDataModle(prod.Id, prod.CustomerName, prod.ProductName, prod.Quantity, prod.DateOfDelivery, (DateTime)prod.DepositDate));
            }

            //生産指示情報及び、生産指示ファイルに出力しなかった受注情報リスト作成
            var reservationData = from prodData in leftOuterJoinQuery
                                  where prodData.DepositDate == null
                                  orderby prodData.Id
                                  select prodData;
            List<OrderFileDataMolde> reservationDataList = new List<OrderFileDataMolde>();
            foreach (var res in reservationData)
            {
                reservationDataList.Add(new OrderFileDataMolde(res.Id, res.CustomerName, res.ProductName, res.Quantity, res.DateOfDelivery));
            }

            return new ProductAndOrderDataModel(prodDataList, reservationDataList);
        }

        /// <summary>
        /// 生産指示情報の一覧を上書き出力する
        /// </summary>
        /// <param name="outPutDataList">生産指示情報情報のリスト</param>
        /// <param name="anOutputPath">出力ファイルパス</param>
        /// <returns>異常終了の場合はエラーコード、それ以外は0</returns>
        private static int OutPutProductOrderFile(List<ProductOrderFileDataModle> outPutDataList, string anOutputPath)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(@anOutputPath, false, ENCODING))
                {
                    foreach (ProductOrderFileDataModle dataModel in outPutDataList)
                    {
                        sr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}",
                            dataModel.Id, dataModel.CustomerName, dataModel.ProductionName, dataModel.Quantity,
                            dataModel.DateOfDelivery, dataModel.DepositDate.ToString(DESPOSIT_OF_INCOME_FORMAT)));
                    }
                }
            }
            catch (Exception)
            {
                return Constants.OUTPUT_ERRER_OF_PRODUCT_ORDER_FILE;
            }
            return Constants.SUCCESS;
        }

        /// <summary>
        /// 受注情報の一覧を退避ファイルに上書き出力する
        /// </summary>
        /// <param name="outPutDataList">受注情報のリスト</param>
        /// <param name="anOutputPath">出力ファイルパス</param>
        /// <returns>異常終了の場合はエラーコード、それ以外は0</returns>
        private static int OutPutOrderFile(List<OrderFileDataMolde> outPutDataList, string anOutputPath)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter(@anOutputPath, false, ENCODING))
                {
                    foreach (OrderFileDataMolde dataModel in outPutDataList)
                    {
                        sr.WriteLine(string.Format("{0},{1},{2},{3},{4}", dataModel.Id, dataModel.CustomerName, dataModel.ProductionName, dataModel.Quantity, dataModel.DateOfDelivery));
                    }
                }
            }
            catch (Exception)
            {
                return Constants.OUTPUT_ERRER_OF_RESERVATION_FILE;
            }
            return Constants.SUCCESS;
        }

        /// <summary>
        /// 指定したファイルを指定したフォルダに移動する
        /// </summary>
        /// <param name="filePathList">移動ファイルリスト</param>
        /// <param name="folderDir">移動先フォルダ</param>
        /// <returns>異常終了の場合はエラーコード、それ以外は0</returns>
        private static int MoveFiles(List<string> filePathList, String folderDir)
        {
            try
            {
                foreach (string @failPath in filePathList)
                {
                    string fileName = Path.GetFileName(failPath);
                    //ファイルのコピー
                    File.Copy(@failPath, @folderDir + "/" + fileName, true);
                    //コピー元のファイルの削除
                    File.Delete(@failPath);
                }
            }
            catch (Exception)
            {
                return Constants.ERRER_OF_BACKUP;
            }
            return Constants.SUCCESS;
        }


        /// <summary>
        /// 情報データモデルのBaseClass
        /// </summary>
        private class BaseFileDataModel : IComparable
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="id">受注ID </param>
            public BaseFileDataModel(string id)
            {
                this.Id = id;
            }

            /// <summary>受注ID</summary>
            public string Id { get; set; }

            /// <summary>
            /// 現在のインスタンスの受注IDを同じ型の別のオブジェクトの受注IDと比較し、現在のインスタンスの並べ替え順序での位置が、比較対象のオブジェクトと比べて前か、後か、または同じかを示す整数を返します。
            /// </summary>
            /// <param name="obj">インスタンスと比較するオブジェクト</param>
            /// <returns>自分自身がobjより小さいときはマイナスの数、大きいときはプラスの数、同じときは0を返す</returns>
            public int CompareTo(object obj)
            {
                //nullの場合
                if (obj == null)
                {
                    return 1;
                }

                //違う型の場合
                if (this.GetType() != obj.GetType())
                {
                    return 1;
                }

                return this.Id.CompareTo(((BaseFileDataModel)obj).Id);
            }
        }

        /// <summary>
        /// 受注情報データモデル
        /// </summary>
        private class OrderFileDataMolde : BaseFileDataModel
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="id">受注ID</param>
            /// <param name="customerName">顧客名</param>
            /// <param name="productName">製品名</param>
            /// <param name="quantity">数量</param>
            /// <param name="dateOfDelivery">納期</param>
            public OrderFileDataMolde(string id, string customerName, string productName, int quantity, string dateOfDelivery)
                : base(id)
            {
                this.CustomerName = customerName;
                this.ProductionName = productName;
                this.Quantity = quantity;
                this.DateOfDelivery = dateOfDelivery;
            }

            /// <summary>顧客名</summary>
            public string CustomerName { get; set; }

            /// <summary>製品名</summary>
            public string ProductionName { get; set; }

            /// <summary>数量</summary>
            public int Quantity { get; set; }

            /// <summary>納期</summary>
            public string DateOfDelivery { get; set; }
        }

        /// <summary>
        /// 入金情報データモデル
        /// </summary>
        private class IncomeFileDataModel : BaseFileDataModel
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="id">受注ID</param>
            /// <param name="depositDate">入金日時</param>
            public IncomeFileDataModel(string id, DateTime? depositDate)
                : base(id)
            {
                this.DepositDate = depositDate;
            }

            /// <summary>入金日時</summary>
            public DateTime? DepositDate { get; set; }
        }

        /// <summary>
        /// 生産指示情報データモデル
        /// </summary>
        private class ProductOrderFileDataModle : OrderFileDataMolde
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="id">受注ID</param>
            /// <param name="depositDate">入金日時</param>
            public ProductOrderFileDataModle(string id, string customerName, string productName, int quantity, string dateOfDelivery, DateTime depositDate)
                : base(id, customerName, productName, quantity, dateOfDelivery)
            {
                this.DepositDate = depositDate;
            }

            /// <summary>入金日時</summary>
            public DateTime DepositDate { get; set; }
        }

        /// <summary>
        /// 生産指示情報及び、生産指示ファイルに出力しなかった受注情報のデータモデル
        /// </summary>
        private class ProductAndOrderDataModel
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="outPutProductOrderDataList">生産指示情報リスト</param>
            /// <param name="outPutOrderDataList">生産指示ファイルに出力しなかった受注情報リスト</param>
            public ProductAndOrderDataModel(List<ProductOrderFileDataModle> outPutProductOrderDataList, List<OrderFileDataMolde> outPutOrderDataList)
            {
                this.OutPutProductionDataList = outPutProductOrderDataList;
                this.OutPutOrderDataList = outPutOrderDataList;
            }

            /// <summary>生産指示ファイルに出力しなかった受注情報リスト</summary>
            public List<OrderFileDataMolde> OutPutOrderDataList { get; set; }

            /// <summary>生産指示情報リスト</summary>
            public List<ProductOrderFileDataModle> OutPutProductionDataList { get; set; }
        }
    }
}
