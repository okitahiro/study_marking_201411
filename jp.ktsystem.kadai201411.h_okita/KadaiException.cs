using System;

namespace jp.ktsystem.kadai201411.h_okita
{
    /// <summary>
    /// Kadai用例外クラス
    /// </summary>
    public class KadaiException: Exception
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="errorCode">エラーコード</param>
        public KadaiException(ErrorCode errorCode)
        {
            _errorCode = errorCode;
        }

        /// <summary>
        /// エラーコード
        /// </summary>
        public ErrorCode ErrorCode
        { 
            get
            {
                return _errorCode;
            }
        }
        private ErrorCode _errorCode;
    }

    /// <summary>
    /// エラーコードの列挙型
    /// </summary>
    public enum ErrorCode 
    {
        /// <summary>受注情報ファイル入力エラー</summary>
        INPUT_ERRER_OF_ORDER_FILE = 1,
        /// <summary>受注情報ファイルフォーマットエラー</summary>
        FORMAT_ERRER_OF_ORDER_FILE = 2,
        /// <summary>入金情報ファイル入力エラー</summary>
        INPUT_ERRER_OF_INCOME_FILE = 3,
        /// <summary>入金情報ファイルフォーマットエラー</summary>
        FORMAT_ERRER_OF_INCOME_FILE = 4,
        /// <summary>生産指示ファイル出力エラー</summary>
        OUTPUT_ERRER_OF_PRODUCTION_INSTRUCTION_FILE = 5,
        /// <summary>退避ファイル入力エラー</summary>
        INPUT_ERRER_OF_SAVING_FILE = 6,
        /// <summary>退避ファイル出力エラー</summary>
        OUTPUT_ERRER_OF_SAVING_FILE = 7,
        /// <summary>問1のファイル出力エラー</summary>
        OUTPUT_ERRER_OF_FIRST_QUESTION = 8,
        /// <summary>問1のファイル出力エラー</summary>
        ERRER_OF_BACKUP = 9,
        /// <summary>計算エラー(オーバーフロー等)</summary>
        ERRER_OF_CALCULATION = 10
    };
}
