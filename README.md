
# 2014年11月度課題 受注管理システムの構築 #


ある会社で受注管理システムを構築することになった。

この会社での受注の流れは次のようになっている。

1. 顧客が受注システムに対して注文を行う。
2. 受注システムが受注管理システムに対して受注情報を出力する。  
同時に、受注システムが請求管理システムへ請求情報を出力する。
3. 請求管理システムは顧客に対して請求を行う。
4. 顧客は請求に従って入金を行う。
5. 請求管理システムは入金の確認後、  
受注管理システムに対して入金情報を出力する。
6. 受注管理システムは受注情報と入金情報がそろった時点で  
生産管理システムに対して生産指示情報を出力する。
7. 生産管理システムは生産指示情報を受け取ったら  
請求管理システムに対して生産開始情報を送信する。
8. 請求管理システムは生産開始情報を受け取ったら入金情報を消し込む。  
消し込まれた情報は入金情報に含まれなくなる。

このうち受注システム、請求管理システム、生産管理は既存であり、  
受注管理システムのみ構築するものとする。

この流れを参考に次のようなプログラムを作成してください。

## 問1(Lv1,2) 受注情報に存在する製品と受注数量の一覧を出力する ##

受注情報ファイルを読み込み、製品名ごとに数量を集計して出力する関数を作成してください。

- 受注情報ファイルはCSVフォーマットのファイルで出力されます。
- 受注情報ファイルのファイル名はorderで開始され、拡張子は.txtになります。
- 受注情報ファイルの入力ディレクトリは関数の引数で渡されるものとします。
- 受注情報ファイルのエンコードはUTF-8とします。
- 受注情報ファイルのフィールドは次の通りです。  

	*[受注ID],[顧客名],[製品名],[数量],[納期]*  
　*受注ID：受注システムで採番されるユニークなID。必須*  
　*顧客名：注文した顧客名。必須*  
　*製品名：受注した製品の名称。必須*  
　*数量：受注した数。半角数字で正の整数のみ。必須*  
　*納期：納期をyyyyMMdd形式で出力。空白可能*  

- 受注IDが重複した場合は後に出力されているほうを正としてください。  
- 必須項目が空白の場合及びフォーマットが指定されている項目のフォーマットが  
間違っている場合はエラーとして処理を中断してください。
- 入力ディレクトリに指定されたディレクトリに複数の受注情報ファイルが存在した場合は  
すべての情報を合算して出力してください。
- 入力ディレクトリに指定されたディレクトリが存在しない場合はエラーとしてください。
- ファイルの処理順はファイル名（大文字小文字を区別しない）でのソート順としてください。  
linuxなど大文字小文字を区別しない場合は  
同一のファイルが存在できる場合は第二ソート条件として  
大文字小文字を区別したファイル名を使用してください。
- 出力はCSVフォーマットのファイルで行ってください。
- 出力ファイル名は"ordercount.out"としてください
- 出力ディレクトリは関数の引数で渡されるものとします。
- 出力ファイルのエンコードはUTF-8とします。
- 出力ファイルのフィールドは以下のようにしてください。

	*[製品名],[数量]*  
　*製品名：受注情報ファイルに記載された製品名*  
　*数量：受注情報ファイルに記載された該当製品の数量の合計*  

- 出力ファイルへの出力順は製品名の昇順とします。
- 出力ディレクトリに指定されたディレクトリが存在しない場合はエラーとしてください。

## 問2(Lv.2) 受注情報と入金情報を元に生産指示情報を出力する ##

受注情報ファイルと入金情報ファイルを読み込み、両方に存在する受注IDをもつ情報を
生産指示ファイルに出力するプログラムを作成してください。

- 入金情報ファイルはCSVフォーマットのファイルで出力されます。
- 入金情報ファイルのファイル名は"income.txt"になります。
- 入金情報ファイルの入力ディレクトリは関数の引数で渡されるものとします。
- 入金情報ファイルのエンコードはUTF-8とします。
- 入金情報ファイルのフィールドは次の通りです。

	*[受注ID],[入金日時]*  
　*受注ID：受注システムで採番されるユニークなID。必須*  
　*入金日時：入金された日時ををyyyyMMddHHmmss形式で出力。必須*  

- 受注IDが重複した場合は入金日時が早いほうを正としてください。
- 必須項目が空白の場合及びフォーマットが指定されている項目のフォーマットが  
間違っている場合はエラーとして処理を中断してください。

- 入金情報ファイルは請求管理システムが更新しますので、  
受注管理システムでは書き込みを行わないでください。
- 受注情報に関しては問1と同様です。
- 入金情報ファイルおよび受注情報ファイルの入力ディレクトリに指定されたディレクトリが  
存在しない場合はエラーとしてください。
- 生産指示情報の出力はCSVフォーマットのファイルで行ってください。
- 生産指示ファイル名は"productorder.out"としてください。
- 生産指示ファイルの出力ディレクトリは関数の引数で渡されるものとします。
- 生産指示ファイルのエンコードはUTF-8とします。
- 生産指示ファイルのフィールドは以下のようにしてください。

	*[受注ID],[顧客名],[製品名],[数量],[納期],[入金日時]*  
　*受注ID：受注情報ファイルに記載された受注ID*  
　*顧客名：受注情報ファイルに記載された顧客名*  
　*製品名：受注情報ファイルに記載された製品名*  
　*数量：受注情報ファイルに記載された数量*  
　*納期：受注情報ファイルに記載された納期。受注情報ファイルの納期が空白の場合は空白*  
　*入金日時：入金情報ファイルに記載された入金日時*  
　
- 生産指示ファイルの出力順は入金日時の昇順とし、入金日時が同じ場合は受注IDの昇順とします。
- 出力ディレクトリに指定されたディレクトリが存在しない場合はエラーとしてください。
- 生産指示ファイルに出力しなかった受注情報が存在した場合、  
出力されなかった受注情報をファイルに退避させ、  
次回実行時に受注情報の先頭に記載されているものとして処理を行ってください。
- 退避させるファイル名は"reservation.dat"とします。
- 退避させるファイルを配置するディレクトリは自由としますが、以下の制約を守ってください。

　　　1. 実行環境が変わっても間違いなく入出力できるディレクトリに配置してください。  
　　　2. ルートディレクトリに配置しないでください。  
　　　3. 環境変数および実行フォルダが同じなら同じフォルダに配置するようにしてください。  

　　　また、配置するディレクトリを返す関数を作成してください。  

- 退避させるファイルのフォーマットは定めません。扱いやすいフォーマットにしてください。
- 初回起動時には退避させるファイルが存在しない場合は0件の情報が記録された  
退避させたファイルが存在した場合と同様に動作するようにしてください。
- 関数終了時に処理を行った受注情報ファイルをすべてバックアップディレクトリに移動させてください。
- バックアップディレクトリは関数の引数で指定されるものとします。

## 附）関数の定義 ##

関数の定義は以下のとおりとします。

----------

    // java
    public class Kadai {
    	// 問1
    	public static int countOrder(String anOrderFileDir, String anOutputDir);
    	
    	// 問2
    	public static int createProductOrder(
    		String anOrderFileDir, String anIncomeFileDir, String anOutputDir,
    		String aBackupDir);
    	
    	// 退避ファイル出力ディレクトリ取得
    	public static string getReservationFileDir();
    }

----------

    // C#
    public class Kadai
    {
    	// 問1
    	public static int CountOrder(string anOrderFileDir, string anOutputDir);
    	
    	// 問2
    	public static int CreateProductOrder(
    		string anOrderFileDir, string anIncomeFileDir, string anOutputDir,
    		string aBackupDir);
    	
    	// 退避ファイル出力ディレクトリ取得
    	public static string GetReservationFileDir();
    }

----------

引数：  
　orderFileDir : 受注情報ファイル入力ディレクトリ  
　incomeFileDir : 入金情報ファイル入力ディレクトリ  
　outputDir : 出力ディレクトリ  
　backupDir : バックアップファイル出力ディレクトリ  

戻り値：  
　問1および問2 : 出力したレコードの件数。異常終了の場合はエラーコード  
　退避ファイル出力ディレクトリ取得 : 退避ファイル出力ディレクトリ  


## 附）エラーコード一覧 ##

1 : 受注情報ファイル入力エラー  
2 : 受注情報ファイルフォーマットエラー  
3 : 入金情報ファイル入力エラー  
4 : 入金情報ファイルフォーマットエラー  
5 : 生産指示ファイル出力エラー  
6 : 退避ファイル入力エラー  
7 : 退避ファイル出力エラー  
8 : 問1のファイル出力エラー  
9 : バックアップ失敗  
10: 計算エラー(オーバーフロー等)  