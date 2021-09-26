
namespace ColorFormulaSearchTool.DB
{
    //Sql相关语句
    public class SqlList
    {
        //根据SQLID返回对应的SQL语句  
        private string _result;

        /// <summary>
        /// 配方点击率查询报表(配方系统使用)
        /// </summary>
        /// <param name="sdt">开始日期</param>
        /// <param name="edt">结束日期</param>
        /// <param name="brandname">品牌</param>
        /// <param name="colorcode">色母编码</param>
        /// <returns></returns>
        public string Get_SearchColorCodeReport(string sdt,string edt,string brandname,string colorcode)
        {
            _result = $@"
                            SELECT X.内部色号,X.产品系列,X.品牌,SUM(X.Click) 点击次数 
	                        INTO #TEMP
	                        FROM (
			                        SELECT  A.InnerColorCode 内部色号,A.ProductName 产品系列,A.BrandName 品牌,1 Click
			                        FROM dbo.NoFormulaClickCankings A
			                        WHERE CONVERT(VARCHAR(100),A.Time,23)>=CONVERT(VARCHAR(100),'{sdt}',23)
			                        AND CONVERT(VARCHAR(100),A.Time,23)<=CONVERT(VARCHAR(100),'{edt}',23)
			                        AND (A.BrandName='{brandname}' OR '{brandname}'='')
			                        AND A.ProductName IN('1K Basecoat','2K Topcoat')
			                        AND LEN(ISNULL(A.InnerColorCode,0))>1
	                        )X
	                        GROUP BY X.内部色号,X.产品系列,X.品牌
	                        ORDER BY X.内部色号

	                        --SELECT * FROM #TEMP A WHERE A.内部色号='ALF00024'

                            --通过#TEMP0与AutoPaint_Integrated库进行连接
	                        SELECT A.InnerColorId,
	                                Z.品牌,Z.产品系列,
	                                A.ColorCode 内部色号,A.ColorName 颜色名称,
		                            X2.AutoName 制造商,X1.AutoName 车型,
		                            I.StandardColorCode 色号,B1.FormulaVersionDate 版本日期,Z.点击次数
	                        INTO #TEMP1
	                        FROM AutoPaint_Integrated20210528.DBO.InnerColor A
	                        --获取制造商 车型信息
	                        INNER JOIN AutoPaint_Integrated20210528.dbo.ColorAuto x0 ON a.InnerColorId=x0.InnerColorId
                            INNER JOIN AutoPaint_Integrated20210528.dbo.Auto x1 ON x0.AutoId=x1.AutoId
                            INNER JOIN AutoPaint_Integrated20210528.dbo.Auto x2 ON x1.MasterId=x2.AutoId
	                        --色号(标准色号)
	                        LEFT JOIN AutoPaint_Integrated20210528.DBO.StandardColor I ON A.InnerColorId=I.InnerColorId
	                        --版本日期
	                        INNER JOIN AutoPaint_Integrated20210528.dbo.ColorFormula B ON A.InnerColorId=B.InnerColorId AND B.Status !=-1
	                        INNER JOIN AutoPaint_Integrated20210528.dbo.Formula B1 ON B.FormulaId=B1.FormulaId
	                        --获取品牌,产品系列
	                        INNER JOIN AutoPaint_Integrated20210528.DBO.BrandProduct C ON B1.ProductId=C.ProductId
	                        INNER JOIN AutoPaint_Integrated20210528.DBO.Brand C1 ON C.BrandId=C1.BrandId       --品牌
	                        INNER JOIN AutoPaint_Integrated20210528.DBO.Product C2 ON C.ProductId=C2.ProductId --产品系列
	                        --与#TEMP表进行合并
	                        INNER JOIN #TEMP Z ON A.ColorCode=Z.内部色号 AND C2.ProductName=Z.产品系列 AND C1.BrandName=Z.品牌
	                        --WHERE A.ColorCode='ACU001196'

	                        --SELECT * FROM #TEMP1 A WHERE A.内部色号='VOL002492' order by 版本日期
	
	                        --获取内部色号对应的色母明细记录(@ColorantCode参数需要使用;注:若(@ColorantCode参数不为空时使用))
	                        IF('{colorcode}' !='')
	                        BEGIN
		                        SELECT DISTINCT A.InnerColorId,/*D.ColorantCode 色母号,*/F.BrandName 品牌,G.ProductName 产品系列,B1.FormulaVersionDate 版本日期
		                        INTO #TEMP2 
		                        FROM AutoPaint_Integrated20210528.dbo.InnerColor A
		                        INNER JOIN AutoPaint_Integrated20210528.dbo.ColorFormula B ON A.InnerColorId=B.InnerColorId AND B.Status !=-1
		                        INNER JOIN AutoPaint_Integrated20210528.dbo.Formula B1 ON B.FormulaId=B1.FormulaId
		                        INNER JOIN AutoPaint_Integrated20210528.dbo.ColorFormulaColorants C ON B.ColorFormulaId=C.ColorFormulaId
		                        INNER JOIN AutoPaint_Integrated20210528.dbo.Colorants D ON C.ColorantsId=D.ColorantId
		                        --获取品牌,产品系列
	                            INNER JOIN AutoPaint_Integrated20210528.DBO.BrandProduct E ON B1.ProductId=E.ProductId
	                            INNER JOIN AutoPaint_Integrated20210528.DBO.Brand F ON E.BrandId=F.BrandId       --品牌
	                            INNER JOIN AutoPaint_Integrated20210528.DBO.Product G ON E.ProductId=G.ProductId --产品系列
		                        --与#TEMP表进行合并
		                        INNER JOIN #TEMP J ON A.ColorCode=J.内部色号 AND G.ProductName=J.产品系列 AND F.BrandName=J.品牌
		                        WHERE D.ColorantCode = '{colorcode}'
	                        END

	                        --SELECT * FROM #TEMP2 a WHERE a.InnerColorId='98169'

	                        --SELECT * FROM #TEMP1 A WHERE A.内部色号='VOL001512'

	                        --结合(注:若@ColorantCode参数不为空,即将#TEMP2选择出来的结果与#TEMP1进行连接并进行输出)
	                        --注:对接条件:内部色号 品牌 产品系列 版本日期
	                        IF ('{colorcode}' !='')
	                        BEGIN
	                            SELECT DISTINCT A.品牌,A.产品系列,A.内部色号,A.颜色名称,B.制造商,C.车型,D.色号,A.版本日期,A.点击次数 
	                            FROM #TEMP1 A
	                            --将‘制造商’合成一行显示
		                        INNER JOIN (
						                        SELECT X2.InnerColorId,
								                        STUFF((SELECT DISTINCT ','+制造商 FROM dbo.#TEMP1 WHERE InnerColorId=x2.InnerColorId FOR XML PATH('')),1,1,'') AS 制造商
						                        FROM #TEMP1 X2
						                        GROUP BY X2.InnerColorId
					                        ) AS B ON A.InnerColorId=B.InnerColorId
		                        --将‘车型’合成一行显示
		                        INNER JOIN (
						                        SELECT X2.InnerColorId,
								                        STUFF((SELECT DISTINCT ','+车型 FROM dbo.#TEMP1 WHERE InnerColorId=x2.InnerColorId FOR XML PATH('')),1,1,'') AS 车型
						                        FROM #TEMP1 X2
						                        GROUP BY X2.InnerColorId
					                        ) AS C ON A.InnerColorId=C.InnerColorId	
		                        --将‘色号’合成一行显示
		                        INNER JOIN (
						                        SELECT X2.InnerColorId,
								                        STUFF((SELECT DISTINCT ','+色号 FROM dbo.#TEMP1 WHERE InnerColorId=x2.InnerColorId FOR XML PATH('')),1,1,'') AS 色号
						                        FROM #TEMP1 X2
						                        GROUP BY X2.InnerColorId
					                        ) AS D ON A.InnerColorId=D.InnerColorId	
	                            INNER JOIN #TEMP2 B1 ON A.InnerColorId=B1.InnerColorId AND A.品牌=B1.品牌 AND A.产品系列=B1.产品系列 AND a.版本日期=B1.版本日期
	                            ORDER BY A.点击次数 DESC
	                        END
	                        ELSE IF ('{colorcode}'='')
	                        BEGIN
		                        SELECT DISTINCT A.品牌,A.产品系列,A.内部色号,A.颜色名称,B.制造商,C.车型,D.色号,A.版本日期,A.点击次数
		                        FROM #TEMP1 A
		                        --将‘制造商’合成一行显示
		                        INNER JOIN (
					                            SELECT X2.InnerColorId,
					                                    STUFF((SELECT DISTINCT ','+制造商 FROM dbo.#TEMP1 WHERE InnerColorId=x2.InnerColorId FOR XML PATH('')),1,1,'') AS 制造商
					                            FROM #TEMP1 X2
					                            GROUP BY X2.InnerColorId
					                        ) AS B ON A.InnerColorId=B.InnerColorId
		                        --将‘车型’合成一行显示
		                        INNER JOIN (
					                            SELECT X2.InnerColorId,
					                                    STUFF((SELECT DISTINCT ','+车型 FROM dbo.#TEMP1 WHERE InnerColorId=x2.InnerColorId FOR XML PATH('')),1,1,'') AS 车型
					                            FROM #TEMP1 X2
					                            GROUP BY X2.InnerColorId
					                        ) AS C ON A.InnerColorId=C.InnerColorId	
		                        --将‘色号’合成一行显示
		                        INNER JOIN (
					                            SELECT X2.InnerColorId,
					                                    STUFF((SELECT DISTINCT ','+色号 FROM dbo.#TEMP1 WHERE InnerColorId=x2.InnerColorId FOR XML PATH('')),1,1,'') AS 色号
					                            FROM #TEMP1 X2
					                            GROUP BY X2.InnerColorId
					                        ) AS D ON A.InnerColorId=D.InnerColorId	
		                        ORDER BY A.点击次数 DESC
	                        END
                        ";
            return _result;
        }

        /// <summary>
        /// 查询色母单价(K3数据库使用)
        /// </summary>
        /// <param name="brandname">品牌</param>
        /// <param name="sdt">日期开始</param>
        /// <param name="edt">日期结束</param>
        /// <param name="typeid">选择类型(决定是使用‘创建日期’或‘修改日期’进行查询) 0:创建日期 1:修改日期</param>
        /// <returns></returns>
        public string Get_SearchColorantPrice(string brandname,string sdt, string edt, int typeid)
        {
            //色母单价窗体查询时使用
            if (typeid == 0)
            {
                _result = $@" SELECT A.Pid,A.ColorantCode,A.Price,A.CreateDate,A.ChangeDate
                              FROM dbo.T_BD_ColorantPrice A
                              WHERE (SUBSTRING(A.ColorantCode,1,3) LIKE '%{brandname}%' or '{brandname}'='')  --品牌
                              AND CONVERT(VARCHAR(100),A.CreateDate,23) >=CONVERT(VARCHAR(100),'{sdt}',23)
                              AND CONVERT(VARCHAR(100),A.CreateDate,23) <=CONVERT(VARCHAR(100),'{edt}',23)
                            ";
            }
            else if (typeid == 1)
            {
                _result = $@" SELECT A.Pid,A.ColorantCode,A.Price,A.CreateDate,A.ChangeDate
                              FROM dbo.T_BD_ColorantPrice A
                              WHERE (SUBSTRING(A.ColorantCode,1,3) LIKE '%{brandname}%' or '{brandname}'='')  --品牌
                              AND CONVERT(VARCHAR(100),A.ChangeDate,23) >=CONVERT(VARCHAR(100),'{sdt}',23)
                              AND CONVERT(VARCHAR(100),A.ChangeDate,23) <=CONVERT(VARCHAR(100),'{edt}',23)
                            ";
            }
            //初始化及更新(插入)后使用
            else if (brandname == "" && sdt == "" && edt == "")
            {
                _result = $@"SELECT A.Pid,A.ColorantCode,A.Price,A.CreateDate,A.ChangeDate 
                             FROM dbo.T_BD_ColorantPrice A";
            }
            return _result;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public string UpEntry(string tablename)
        {
            switch (tablename)
            {
                case "T_BD_ColorantPrice":
                    _result = @"UPDATE dbo.T_BD_ColorantPrice SET ColorantCode=@ColorantCode,Price=@Price,CreateDate=@CreateDate,ChangeDate=@ChangeDate
                                WHERE PID=@PID";
                    break;
            }
            return _result;
        }

        /// <summary>
        /// 根据表名获取查询表体语句(更新时使用) 只显示TOP 1记录
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string SearchUpdateTable(string tableName)
        {
            _result = $@"
                          SELECT Top 1 a.*
                          FROM {tableName} a
                        ";
            return _result;
        }
    }
}
