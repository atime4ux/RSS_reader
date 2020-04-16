using System;
using System.Collections.Generic;
using System.Text;

namespace libRSSreader
{
    public class clsPaging
    {
        //DATASET의 필요없는 앞부분 잘라내기(현재 3페이지를 보고있다면 1, 2페이지 삭제)
        public System.Data.DataSet cutDS(System.Data.DataSet DS, int pageNum, int pageCnt)
        {
            int max;
            int i;

            //현재 페이지 바로 앞까지의 항목 수 계산
            max = (pageNum - 1) * pageCnt;


            //항목 수 만큼 삭제
            for (i = 0; i < max; i++)
            {
                DS.Tables[0].Rows.RemoveAt(0);
            }

            return DS;
        }



        //페이지 넘버 생성
        public string getPageString(int pageNum, int totalCnt, int pageCnt)
        {
            string Result;
            string cPage;

            StringBuilder strBuilder = new StringBuilder();

            int i;
            int pageByTen;
            int maxPage;
            int cLastpage;
            int cStartPage;

            //리스트가 존재할때 페이지넘버를 표시
            if (totalCnt > 0)
            {
                if (pageCnt == 0)
                {
                    pageCnt = 25;
                }


                //한 화면에 보여줄 페이지 넘버는 10개
                if (pageNum % 10 == 0)
                {
                    pageByTen = pageNum / 10 - 1;
                }
                else
                {
                    pageByTen = pageNum / 10;
                }

                
                //마지막 페이지 번호 계산
                if (totalCnt % pageCnt == 0)
                {
                    maxPage = totalCnt / pageCnt;
                }
                else
                {
                    maxPage = totalCnt / pageCnt + 1;
                }


                //현재 화면에 표시될 마지막 페이지 번호 계산
                //"(pageByTen + 1) * 10" 계산 결과는 maxPage보다 작거나 같을 수 있다
                if ((pageByTen + 1) * 10 < maxPage)
                {
                    cLastpage = (pageByTen + 1) * 10;
                }
                else
                {
                    cLastpage = maxPage;
                }


                //현재 화면에 표시될 첫 페이지 번호 계산
                cStartPage = pageByTen * 10 + 1;

                
                strBuilder.Append("<div id='paging'>");


                //현재 화면의 시작 페이지 번호부터 마지막 페이지 번호까지 수행
                for (i = cStartPage; i <= cLastpage; i++)
                {
                    //출력할 현재 페이지 번호를 cPage에 입력
                    cPage = i.ToString();

                    
                    if ((cStartPage != 1) && (i == cStartPage))
                    {
                        //1페이지로 이동하는 << 생성
                        strBuilder.Append("<a class='paging' href=\"javascript:goPage(" + "1" + "," + pageCnt + ");\">");
                        strBuilder.Append("<<");
                        strBuilder.Append("</a>");
                       // strBuilder.Append("&nbsp;&nbsp;");

                        //현재 화면에 보이는 첫 페이지의 전 페이지로 이동하는 < 생성
                        strBuilder.Append("<a class='paging' href=\"javascript:goPage(" + (cStartPage - 1) + "," + pageCnt + ");\">");
                        strBuilder.Append("<");
                        strBuilder.Append("</a>");
                        //strBuilder.Append("&nbsp;&nbsp;");
                    }


                    //페이지간 구분 기호 삽입
                    if (i > cStartPage)
                    {
                        strBuilder.Append("|");
                    }


                    //표시할 페이지 번호가 현재 페이지면 굵게표시 & 링크 삭제
                    if (cPage.Equals(pageNum.ToString()) == true)
                    {
                        strBuilder.Append("<span id='pageBolder' class='paging'>");
                        strBuilder.Append(cPage);
                        strBuilder.Append("</span>");
                    }
                    else
                    {
                        strBuilder.Append("<a class='paging' href=\"javascript:goPage(" + cPage + "," + pageCnt + ");\">");
                        strBuilder.Append(cPage);
                        strBuilder.Append("</a>");
                    }

                    
                    if ((cLastpage != maxPage) && (i == cLastpage))
                    {
                        //현재 화면에 보이는 마지막 페이지번호의 다음 페이지로 이동하는 > 생성
                        //strBuilder.Append("&nbsp;&nbsp;");
                        strBuilder.Append("<a class='paging' href=\"javascript:goPage(" + (cLastpage + 1) + "," + pageCnt + ");\">");
                        strBuilder.Append(">");
                        strBuilder.Append("</a>");

                        //마지막 페이지로 이동하는 >> 생성
                        //strBuilder.Append("&nbsp;&nbsp;");
                        strBuilder.Append("<a class='paging' href=\"javascript:goPage(" + maxPage + "," + pageCnt + ");\">");
                        strBuilder.Append(">>");
                        strBuilder.Append("</a>");

                    }

                }
                strBuilder.Append("</div>");
            }
            else
            {
                strBuilder.Remove(0, strBuilder.Length);
            }


            Result = strBuilder.ToString();

            return Result;

        }
    }
}