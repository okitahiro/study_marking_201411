﻿using System;
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

        /// <summary>受注情報ファイルのインデックス</summary>
        private enum OrderIndex
        {
            /// <summary>ID</summary>
            ID = 0,
            /// <summary>顧客名</summary>
            CYSTOMER_NAM = 1,
            /// <summary>製品名</summary>
            PRODUCT_NAME = 2,
            /// <summary>数量</summary>
            QUANTITY = 3,
            /// <summary>納期</summary>
            DELIVERY_DATE = 4
        }

        /// <summary>入金情報ファイルのインデックス</summary>
        private enum IncomeIndex
        {
            /// <summary>受注ID</summary>
            ID = 0,
            /// <summary>入金日時</summary>
            DESPOSIT_DATE = 1
        }

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
            if (string.IsNullOrEmpty(anOrderFileDir) || !Directory.Exists(anOrderFileDir))
            {
                return (int)Constants.ErrorCode.INPUT_ERROR_OF_ORDER_FILE;
            }

            //出力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anOutputDir) || !Directory.Exists(anOutputDir))
            {
                return (int)Constants.ErrorCode.OUTPUT_ERROR_OF_FIRST_QUESTION;
            }

            //受注情報のリスト
            List<OrderFileDataModel> orderDataList = null;
            try
            {
                //入力フォルダ内のファイル一覧取得
                List<string> inputFiles = Directory.GetFiles(anOrderFileDir, "order*.txt", SearchOption.TopDirectoryOnly).ToList();
                //ファイル名のソート
                inputFiles.Sort(new OrderFileCompare());

                //受注情報のリストを取得
                orderDataList = GetOrderFileData(inputFiles, false);
            }
            catch (CustomException ce)
            {
                return (int)ce.ErrorNo;
            }
            catch
            {
                return (int)Constants.ErrorCode.INPUT_ERROR_OF_ORDER_FILE;
            }

            //受注情報データから、製品名ごとに数量を集計する
            Dictionary<string, int> outPutData = GetTotalProductionsQuantity(orderDataList);

            try
            {
                OutputOrderCount(outPutData, Path.Combine(anOutputDir, OUTPUT_FILE_NAME));

            }
            catch
            {
                return (int)Constants.ErrorCode.OUTPUT_ERROR_OF_FIRST_QUESTION;
            }

            return outPutData.Count;
        }

        /// <summary>
        /// 問２ 受注情報と入金情報を元に生産指示情報を出力する
        /// </summary>
        /// <param name="anOrderFileDir">受注情報ファイル入力ディレクトリ</param>
        /// <param name="anIncomeFileDir">入金情報ファイル入力ディレクトリ</param>
        /// <param name="anOutputDir">出力ディレクトリ</param>
        /// <param name="aBackupDir">バックアップファイル出力ディレクトリ</param>
        /// <returns>出力したレコードの件数（異常終了の場合はエラーコード）</returns>
        public static int CreateProductOrder(String anOrderFileDir, String anIncomeFileDir, String anOutputDir, String aBackupDir)
        {
            //受注情報ファイル入力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anOrderFileDir) || !Directory.Exists(anOrderFileDir))
            {
                return (int)Constants.ErrorCode.INPUT_ERROR_OF_ORDER_FILE;
            }

            //入金情報ファイル出力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anIncomeFileDir) || !Directory.Exists(anIncomeFileDir))
            {
                return (int)Constants.ErrorCode.INPUT_ERROR_OF_INCOME_FILE;
            }

            //出力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(anOutputDir) || !Directory.Exists(anOutputDir))
            {
                return (int)Constants.ErrorCode.OUTPUT_ERROR_OF_PRODUCT_ORDER_FILE;
            }

            //バックアップファイル出力ディレクトリの存在チェック
            if (string.IsNullOrEmpty(aBackupDir) || !Directory.Exists(aBackupDir))
            {
                return (int)Constants.ErrorCode.ERROR_OF_BACKUP;
            }

            //受注情報フォルダ内の受注情報ファイル一覧
            List<string> inputFiles = null;
            //受注情報のリスト
            List<OrderFileDataModel> orderDataList = null;
            //入力退避ファイル存在フラグ
            bool existsReservation = false;
            try
            {
                //受注情報フォルダ内の受注情報ファイル一覧取得
                inputFiles = Directory.GetFiles(anOrderFileDir, "order*.txt", SearchOption.TopDirectoryOnly).ToList();
                //ファイル名のソート
                inputFiles.Sort(new OrderFileCompare());

                //退避ファイルパス取得
                string reservationFileDir = Path.Combine(GetReservationFileDir(), RESERVATION_FILE_NAME);
                if (!string.IsNullOrEmpty(reservationFileDir) && File.Exists(reservationFileDir))
                {
                    //退避ファイルパスを受注情報ファイル一覧の先頭に追加
                    inputFiles.Insert(0, reservationFileDir);
                    existsReservation = true;
                }

                //受注情報のリストを取得
                orderDataList = GetOrderFileData(inputFiles, existsReservation);
            }
            catch (CustomException ce)
            {
                return (int)ce.ErrorNo;
            }
            catch
            {
                return (int)Constants.ErrorCode.INPUT_ERROR_OF_ORDER_FILE;
            }

            //入金ファイルパス
            string incomeFilePath = Path.Combine(anIncomeFileDir, INCOME_FILE_NAME);
            //入金情報のリストを取得
            List<IncomeFileDataModel> incomeDataList = null;
            try
            {
                incomeDataList = GetIncomeFileData(incomeFilePath);
            }
            catch (CustomException ce)
            {
                return (int)ce.ErrorNo;
            }
            catch
            {
                return (int)Constants.ErrorCode.INPUT_ERROR_OF_INCOME_FILE;
            }

            //入金情報マップ作成
            Dictionary<string, DateTime> incomeDataMap = new Dictionary<string, DateTime>();
            foreach (IncomeFileDataModel income in incomeDataList)
            {
                incomeDataMap.Add(income.Id, income.DepositDate);
            }

            //生産指示情報及び、生産指示ファイルに出力しなかった受注情報のデータを取得する
            ProductAndUnoutputOrderDataModel resultData = GetProductDataAndUnOutputOrderData(orderDataList, incomeDataMap);

            try
            {
                //生産指示出力
                OututProductOrderFile(resultData.OutputProductionDataList, Path.Combine(anOutputDir, PRODUCT_ORDER_FILE_NAME));
            }
            catch
            {
                return (int)Constants.ErrorCode.OUTPUT_ERROR_OF_PRODUCT_ORDER_FILE;
            }

            //退避ディレクトリが存在しない場合、フォルダ作成
            if (0 < resultData.OutputOrderDataList.Count)
            {
                string reservationDir = GetReservationFileDir();
                if (!Directory.Exists(reservationDir))
                {
                    Directory.CreateDirectory(reservationDir);
                }
                //生産指示ファイルに出力しなかった受注情報のデータを出力
                string reservationFilePath = Path.Combine(reservationDir, RESERVATION_FILE_NAME);
                try
                {
                    OutPutOrderFile(resultData.OutputOrderDataList, reservationFilePath);
                }
                catch
                {
                    return (int)Constants.ErrorCode.OUTPUT_ERROR_OF_RESERVATION_FILE;
                }
            }

            //使用した受注情報ファイルをバックアップフォルダへ移動
            if (existsReservation)
            {
                inputFiles.RemoveAt(0);
            }

            try
            {
                MoveFiles(inputFiles, aBackupDir);
            }
            catch
            {
                return (int)Constants.ErrorCode.ERROR_OF_BACKUP;
            }

            //出力した生産指示レコード数を返す
            return resultData.OutputProductionDataList.Count;
        }

        /// <summary>
        /// 退避ファイル出力ディレクトリ取得
        /// </summary>
        /// <returns>退避ファイル出力ディレクトリ</returns>
        public static string GetReservationFileDir()
        {
            //現在作業ディレクトリ
            string currentPath = Directory.GetCurrentDirectory();
            return Path.Combine(currentPath, RESERVATION_FOLDER_NAME);
        }

        /// <summary>
        /// 文字列が半角数字のみかどうか調べる
        /// </summary>
        /// <param name="str">調べる文字列</param>
        /// <returns>半角数字：true、それ以外：false</returns>
        private static bool IsHalfNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return Regex.IsMatch(str, @"^[0-9]+$");
        }

        /// <summary>
        /// 指定された受注情報ファイルから、受注情報のリストを取得する
        /// </summary>
        /// <param name="files">受注情報ファイルパスのリスト</param>
        /// <param name="existsReservation">退避ファイル存在フラグ</param>
        /// <returns>受注情報のリスト</returns>
        private static List<OrderFileDataModel> GetOrderFileData(List<string> files, bool existsReservation)
        {
            //受注情報のリスト
            List<OrderFileDataModel> orderDataList = new List<OrderFileDataModel>();

            int i = 0;
            try
            {
                //ファイル読み込み
                for (i = 0; i < files.Count; i++)
                {

                    // UTF8コードチェック
                    if (!IsUTF8Code(files[i]))
                    {
                        if (existsReservation && 0 == i)
                        {
                            throw new CustomException(Constants.ErrorCode.INPUT_ERROR_OF_RESERVATION_FILE);
                        }
                        else
                        {
                            throw new CustomException(Constants.ErrorCode.INPUT_ERROR_OF_ORDER_FILE);
                        }
                    }

                    using (StreamReader sr = new StreamReader(files[i], ENCODING))
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
                                throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_ORDER_FILE);
                            }

                            //必須フィールドの入力チェック
                            if (string.IsNullOrEmpty(values[(int)OrderIndex.ID]) || string.IsNullOrEmpty(values[(int)OrderIndex.CYSTOMER_NAM]) ||
                                string.IsNullOrEmpty(values[(int)OrderIndex.PRODUCT_NAME]) || string.IsNullOrEmpty(values[(int)OrderIndex.QUANTITY]))
                            {
                                throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_ORDER_FILE);
                            }

                            //数量フィールドが半角数字のみで入力されているかのチェック
                            if (!IsHalfNumber(values[(int)OrderIndex.QUANTITY]))
                            {
                                throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_ORDER_FILE);
                            }

                            //納期のチェック
                            if (string.Empty != values[(int)OrderIndex.DELIVERY_DATE])
                            {
                                //納期の日付フォーマットチェック
                                if (!Regex.IsMatch(values[(int)OrderIndex.DELIVERY_DATE], DELIVERY_OF_ORDER_REG_FORMAT))
                                {
                                    throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_ORDER_FILE);
                                }
                                //納期の日付チェック
                                DateTime date;
                                if (!DateTime.TryParseExact(values[(int)OrderIndex.DELIVERY_DATE], DELIVERY_OF_ORDER_FORMAT,
                                    System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out date))
                                {
                                    throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_ORDER_FILE);
                                }
                            }

                            //受注情報モデルの作成
                            OrderFileDataModel dataModel = new OrderFileDataModel(values[(int)OrderIndex.ID],
                                values[(int)OrderIndex.CYSTOMER_NAM], values[(int)OrderIndex.PRODUCT_NAME],
                                int.Parse(values[(int)OrderIndex.QUANTITY]), values[(int)OrderIndex.DELIVERY_DATE]);

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
            catch (CustomException ce)
            {
                throw ce;
            }
            catch
            {
                if (existsReservation && 0 == i)
                {
                    throw new CustomException(Constants.ErrorCode.INPUT_ERROR_OF_RESERVATION_FILE);
                }
                else
                {
                    throw new CustomException(Constants.ErrorCode.INPUT_ERROR_OF_ORDER_FILE);
                }
            }

            return orderDataList;
        }

        /// <summary>
        /// 受注情報データから、製品名ごとに数量を集計する
        /// </summary>
        /// <param name="orderDataList">受注情報のリスト</param>
        /// <param name="anOutputDir">出力ファイルパス</param>
        private static Dictionary<string, int> GetTotalProductionsQuantity(List<OrderFileDataModel> orderDataList)
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
        private static void OutputOrderCount(Dictionary<string, int> outPutData, string anOutputPath)
        {
            using (StreamWriter sr = new StreamWriter(anOutputPath, false, ENCODING))
            {
                foreach (string key in outPutData.Keys)
                {
                    sr.WriteLine(string.Format("{0},{1}", key, outPutData[key]));
                }
            }
        }

        /// <summary>
        /// 指定された入金情報ファイルから、入金情報のリストを取得する
        /// </summary>
        /// <param name="file">入金ファイルパス</param>
        /// <returns>入金情報のリスト（エラーが発生し処理が中断した場合は、nullが返る）</returns>
        private static List<IncomeFileDataModel> GetIncomeFileData(string filePath)
        {
            //入金情報のリスト
            List<IncomeFileDataModel> incomeDataList = new List<IncomeFileDataModel>();

            if (!File.Exists(filePath))
            {
                return incomeDataList;
            }

            // UTF8コードチェック
            if (!IsUTF8Code(filePath))
            {
                throw new CustomException(Constants.ErrorCode.INPUT_ERROR_OF_INCOME_FILE);
            }

            //ファイル読み込み
            using (StreamReader sr = new StreamReader(filePath, ENCODING))
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
                        throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_INCOME_FILE);
                    }

                    //必須フィールドの入力チェック
                    if (string.IsNullOrEmpty(values[(int)IncomeIndex.ID]) || string.IsNullOrEmpty(values[(int)IncomeIndex.DESPOSIT_DATE]))
                    {
                        throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_INCOME_FILE);
                    }

                    //入金日時のフォーマットチェック
                    if (!Regex.IsMatch(values[(int)IncomeIndex.DESPOSIT_DATE], DESPOSIT_OF_INCOME_REG_FORMAT))
                    {
                        throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_INCOME_FILE);
                    }
                    //入金日時のDateTime変換
                    DateTime despositDate;
                    if (!DateTime.TryParseExact(values[(int)IncomeIndex.DESPOSIT_DATE], DESPOSIT_OF_INCOME_FORMAT,
                        System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, out despositDate))
                    {
                        throw new CustomException(Constants.ErrorCode.FORMAT_ERROR_OF_INCOME_FILE);
                    }

                    //入金情報モデルの作成
                    IncomeFileDataModel dataModel = new IncomeFileDataModel(values[(int)IncomeIndex.ID], despositDate);

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

            return incomeDataList;
        }

        /// <summary>
        /// 生産指示情報及び、生産指示ファイルに出力しなかった受注情報を取得する
        /// </summary>
        /// <param name="orderData">受注情報</param>
        /// <param name="incomeDataMap">入金情報</param>
        /// <returns>生産指示情報及び、生産指示ファイルに出力しなかった受注情報</returns>
        private static ProductAndUnoutputOrderDataModel GetProductDataAndUnOutputOrderData(List<OrderFileDataModel> orderDataList, Dictionary<string, DateTime> incomeDataMap)
        {
            List<ProductFileDataModel> productDataList = new List<ProductFileDataModel>();
            List<OrderFileDataModel> unoutputOrderDataList = new List<OrderFileDataModel>();

            foreach (OrderFileDataModel order in orderDataList)
            {
                if (incomeDataMap.ContainsKey(order.Id))
                {
                    productDataList.Add(new ProductFileDataModel(order.Id, order.CustomerName,
                        order.ProductionName, order.Quantity, order.DateOfDelivery, incomeDataMap[order.Id]));
                }
                else
                {
                    unoutputOrderDataList.Add(order);
                }
            }

            productDataList.Sort(new ProductFileDataModelComparer());

            return new ProductAndUnoutputOrderDataModel(productDataList, unoutputOrderDataList);
        }

        /// <summary>
        /// 生産指示情報の一覧を上書き出力する
        /// </summary>
        /// <param name="orderFileDataList">受注情報データリスト</param>
        /// <param name="anOutputPath">出力ファイルパス</param>
        private static void OututProductOrderFile(List<ProductFileDataModel> productDataList, string anOutputPath)
        {
            using (StreamWriter sr = new StreamWriter(anOutputPath, false, ENCODING))
            {
                foreach (ProductFileDataModel dataModel in productDataList)
                {
                    sr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}",
                        dataModel.Id, dataModel.CustomerName, dataModel.ProductionName, dataModel.Quantity,
                        dataModel.DateOfDelivery, dataModel.DepositDate.ToString(DESPOSIT_OF_INCOME_FORMAT)));
                }
            }
        }

        /// <summary>
        /// 受注情報の一覧を退避ファイルに上書き出力する
        /// </summary>
        /// <param name="outPutDataList">受注情報のリスト</param>
        /// <param name="anOutputPath">出力ファイルパス</param>
        private static void OutPutOrderFile(List<OrderFileDataModel> outPutDataList, string anOutputPath)
        {
            using (StreamWriter sr = new StreamWriter(anOutputPath, false, ENCODING))
            {
                foreach (OrderFileDataModel dataModel in outPutDataList)
                {
                    sr.WriteLine(string.Format("{0},{1},{2},{3},{4}", dataModel.Id, dataModel.CustomerName, dataModel.ProductionName, dataModel.Quantity, dataModel.DateOfDelivery));
                }
            }
        }

        /// <summary>
        /// 指定したファイルを指定したフォルダに移動する
        /// </summary>
        /// <param name="filePathList">移動ファイルリスト</param>
        /// <param name="folderDir">移動先フォルダ</param>
        private static void MoveFiles(List<string> filePathList, String folderDir)
        {
            foreach (string failPath in filePathList)
            {
                string fileName = Path.GetFileName(failPath);
                //ファイルのコピー
                File.Copy(failPath, Path.Combine(folderDir, fileName), true);
                //コピー元のファイルの削除
                File.Delete(failPath);
            }
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
            /// 現在のインスタンスの受注IDを同じ型の別のオブジェクトの受注IDと比較し、
            /// 現在のインスタンスの並べ替え順序での位置が、
            /// 比較対象のオブジェクトと比べて前か、後か、または同じかを示す整数を返します。
            /// </summary>
            /// <param name="obj">インスタンスと比較するオブジェクト</param>
            /// <returns>自身の受注IDがobjの受注IDより小さいときはマイナスの数、大きいときはプラスの数、同じときは0を返す</returns>
            public int CompareTo(object obj)
            {
                BaseFileDataModel model = obj as BaseFileDataModel;
                //null,または型が異なる場合
                if (null == model)
                {
                    return 1;
                }

                return this.Id.CompareTo(model.Id);
            }
        }

        /// <summary>
        /// 受注情報データモデル
        /// </summary>
        private class OrderFileDataModel : BaseFileDataModel
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="id">受注ID</param>
            /// <param name="customerName">顧客名</param>
            /// <param name="productName">製品名</param>
            /// <param name="quantity">数量</param>
            /// <param name="dateOfDelivery">納期</param>
            public OrderFileDataModel(string id, string customerName, string productName, int quantity, string dateOfDelivery)
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
            public IncomeFileDataModel(string id, DateTime depositDate)
                : base(id)
            {
                this.DepositDate = depositDate;
            }

            /// <summary>入金日時</summary>
            public DateTime DepositDate { get; set; }
        }

        /// <summary>
        /// 生産指示情報データモデル
        /// </summary>
        private class ProductFileDataModel : OrderFileDataModel
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="id">受注ID</param>
            /// <param name="customerName">顧客名</param>
            /// <param name="productName">製品名</param>
            /// <param name="quantity">数量</param>
            /// <param name="dateOfDelivery">納期</param>
            /// <param name="depositDate">入金日時</param>
            public ProductFileDataModel(string id, string customerName, string productName, int quantity, string dateOfDelivery, DateTime depositDate)
                : base(id, customerName, productName, quantity, dateOfDelivery)
            {
                this.DepositDate = depositDate;
            }

            /// <summary>入金日時</summary>
            public DateTime DepositDate { get; set; }
        }

        /// <summary>
        /// ProductFileDataModelソート用のIComparer実装クラス
        /// </summary>
        private class ProductFileDataModelComparer : IComparer<ProductFileDataModel>
        {
            /// <summary>
            /// aの入金日時が早ければ負の数、遅ければ正の数
            /// 一致していれば、受注IDのString.CompareToにて比較した値を返す
            /// </summary>
            public int Compare(ProductFileDataModel a, ProductFileDataModel b)
            {
                int res = a.DepositDate.CompareTo(b.DepositDate);
                if (0 == res)
                {
                    res = a.Id.CompareTo(b.Id);
                }
                return res;
            }
        }

        /// <summary>
        /// 生産指示情報及び、生産指示ファイルに出力しなかった受注情報のデータモデル
        /// </summary>
        private class ProductAndUnoutputOrderDataModel
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="outPutProductOrderDataList">生産指示情報リスト</param>
            /// <param name="outPutOrderDataList">生産指示ファイルに出力しなかった受注情報リスト</param>
            public ProductAndUnoutputOrderDataModel(List<ProductFileDataModel> outPutProductOrderDataList, List<OrderFileDataModel> outPutOrderDataList)
            {
                this.OutputProductionDataList = outPutProductOrderDataList;
                this.OutputOrderDataList = outPutOrderDataList;
            }

            /// <summary>生産指示ファイルに出力しなかった受注情報リスト</summary>
            public List<OrderFileDataModel> OutputOrderDataList { get; set; }

            /// <summary>生産指示情報リスト</summary>
            public List<ProductFileDataModel> OutputProductionDataList { get; set; }
        }

        /// <summary>
        /// Kadai用カスタムエラー通知用Exception
        /// </summary>
        private class CustomException : Exception
        {
            /// <summary>
            /// エラー番号
            /// </summary>
            public Constants.ErrorCode ErrorNo
            {
                get
                {
                    return _errorNo;
                }
            }
            private Constants.ErrorCode _errorNo;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="errorNo">エラー番号</param>
            public CustomException(Constants.ErrorCode errorNo)
            {
                _errorNo = errorNo;
            }
        }

        // 文字コード（改行）
		public static readonly int N_CODE = 0x0A;
		// 文字コード（キャリッジリターン）
        public static readonly int R_CODE = 0x0D;

        /// <summary>
        /// ファイルのバイト配列が改行、キャリッジリターン以外の制御文字を含まないUTF8コードのみで作成されているかどうかを判定する
        /// ※）UTF8用関数
        /// </summary>
        /// <param name="url">ファイルパス</param>
        /// <returns>UTF8コードのみ：true、それ以外：false</returns>
        private static bool IsUTF8Code(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return true;
            }

            byte[] data = null;
            using (FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
            }

            //byte[] data = ENCODING.GetBytes(str);
            for(int i = 0; i < data.Count(); i++)
            {
                byte firstB = data[i];

                if (0x00 <= firstB && 0x7f >= firstB)
                {
                    // 1バイト文字

                    // 1バイト制御文字
                    if ((0x00 <= firstB && 0x1f >= firstB) || (0x7F == firstB))
                    {
                        if (N_CODE != firstB && R_CODE != firstB)
                        {
                            return false;
                        }
                    }
                }
                else if ((0xc0 <= firstB && 0xcf >= firstB) || (0xd0 <= firstB && 0xdf >= firstB))
                {
                    // 2バイト文字
                    if (i >= data.Count() - 1)
                    {
                        return false;
                    }
                    i++;
                    byte secondB = data[i];
                    // 2バイト制御文字
                    if (0xc2 == firstB)
                    {
                        if (0x80 <= secondB && 0xcf >= 0xa0)
                        {
                            return false;
                        }
                    }

                    if (!IsUTF8CodeAfterFirstByte(secondB))
                    {
                        return false;
                    }
                }
                else if (0xe0 <= firstB && 0xef >= firstB)
                {
                    // 3バイト文字
                    if (!IsUTF8NByteChar(3, i, data))
                    {
                        return false;
                    }
                    i += 3 - 1;
                }
                else if (0xf0 <= firstB && 0xf7 >= firstB)
                {
                    // 4バイト文字
                    if (!IsUTF8NByteChar(4, i, data))
                    {
                        return false;
                    }
                    i += 4 - 1;
                }
                else if (0xf8 <= firstB && 0xfb >= firstB)
                {
                    // 5バイト文字
                    if (!IsUTF8NByteChar(5, i, data))
                    {
                        return false;
                    }
                    i += 5 - 1;
                }
                else if (0xfc <= firstB && 0xff >= firstB)
                {
                    // 6バイト文字
                    if (!IsUTF8NByteChar(6, i, data))
                    {
                        return false;
                    }
                    i += 6 - 1;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// UTF8の多バイト文字の2バイト以降が適正かどうか判断する
        /// ※）UTF8用関数
        /// </summary>
        /// <param name="n">UTF8文字が何バイトかどうかを指定する</param>
        /// <param name="i">チェックするバイト配列中の現在のインデックス</param>
        /// <param name="data">チェックするバイト配列</param>
        /// <returns>UTF8の多バイト文字として適正である：true、適正ではない：false</returns>
        private static bool IsUTF8NByteChar(int n, int i, byte[] data)
        {
            if (i >= data.Count() - (n-1))
            {
                return false;
            }

            for (int j = 1; j < n; j++)
            {
                if (!IsUTF8CodeAfterFirstByte(data[i+j]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// UTF8の多バイト文字の２バイト目以降のバイトかどうか判定する
        /// ※）UTF8用関数
        /// </summary>
        /// <param name="b">判定するバイト</param>
        /// <returns>UTF8の多バイト文字の２バイト目以降のバイト：true、それ以外：false</returns>
        private static bool IsUTF8CodeAfterFirstByte(byte b)
        {
            return ((0x80 <= b && 0x8f >= b) || (0x90 <= b && 0x9f >= b) || (0xa0 <= b && 0xaf >= b) || (0xb0 <= b && 0xbf >= b));
        }

        /// <summary>
        /// 受注ファイルのソート用IComparer実装クラス
        /// </summary>
        private class OrderFileCompare : IComparer<string>
        {
            /// <summary>
            /// 文字列aと文字列bを大文字に変換し、
            /// String.CompareToメソッドにより比較し、
            /// 結果が0以外の場合は、そのまま値を返す。
            /// 結果が0の場合は、大文字変換前の文字列を
            /// はじめの文字から順にASCIIコードで比較し、
            /// その差を返す
            /// </summary>
            public int Compare(string a, string b)
            {
                string upperA = a.ToUpper();
                string upperB = b.ToUpper();
                int res = upperA.CompareTo(upperB);
                if (res == 0)
                {
                    for (int i = 0; i < a.Length; i++)
                    {
                        int dif = a[i] - b[i];
                        if (dif != 0)
                        {
                            return dif;
                        }
                    }
                }
                return res;
            }
        }
    }
}
