using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace libRSSreader
{
    
    public class clsFeed
    {
        private System.Collections.ArrayList arrList = new System.Collections.ArrayList();

        public string siteTitle;
        public string siteURL;
        
        public clsFeed()
        {
            siteTitle = "";
            siteURL = "";
        }

        /// <summary>
        /// 아이템 수
        /// </summary>
        public int Count
        {
            get { return arrList.Count; }
        }

        /// <summary>
        /// 읽지 않은 아이템
        /// </summary>
        public clsFeed UnreadItems()
        {
            clsFeed objReturn_Feed = new clsFeed();

            int i;
            for (i = 0; i < this.Count; i++)
            {
                if (!this[i].isRead)
                {
                    objReturn_Feed.addFeed(this[i]);
                }
            }
            return objReturn_Feed;
        }

        /// <summary>
        /// RSS_idx에 해당하는 아이템
        /// </summary>
        public clsFeed extractItems(string RSS_idx)
        {
            clsFeed objReturn_Feed = new clsFeed();

            int i;
            for (i = 0; i < this.Count; i++)
            {
                if (this[i].RSS_idx.Equals(RSS_idx))
                {
                    objReturn_Feed.addFeed(this[i]);
                }
            }

            return objReturn_Feed;
        }

        /// <summary>
        /// 관심 항목 설정된 아이템
        /// </summary>
        public clsFeed FavorItems()
        {
            clsFeed objReturn_Feed = new clsFeed();

            int i;
            for (i = 0; i < this.Count; i++)
            {
                if (this[i].isFavor)
                {
                    objReturn_Feed.addFeed(this[i]);
                }
            }

            return objReturn_Feed;
        }

        public itemInfo this[int index]
        {
            get
            {
                return (itemInfo)arrList[index];
            }
            set
            {
                arrList[index] = (itemInfo)value;
            }
        }

        /// <summary>
        /// 값을 추가
        /// </summary>
        public void addFeed(itemInfo objItem)
        {
            arrList.Add(objItem);
        }

        /// <summary>
        /// 값을 추가
        /// </summary>
        public void addFeed(string RSS_idx, string title, string url, string desc, DateTime date, bool favor, bool read)
        {
            itemInfo objItem = new itemInfo(RSS_idx, title, url, desc, date, favor, read);

            arrList.Add(objItem);
        }

        /// <summary>
        /// 날짜 재림차순으로 정렬
        /// </summary>
        public void sortByDate()
        {
            IComparer myCompare = new CompareItem();

            arrList.Sort(myCompare);
        }

        //비교클래스
        private class CompareItem : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x is itemInfo && y is itemInfo)
                {
                    return -1 * DateTime.Compare(((itemInfo)x).Item_date, ((itemInfo)y).Item_date);
                }

                throw new ArgumentException("object is not a itemInfo");
            }
        }
                
    }

    public class itemInfo
    {
        public string RSS_idx;
        public string Item_title;
        public string Item_url;
        public string Item_desc;
        public DateTime Item_date;
        public bool isFavor;
        public bool isRead;

        public itemInfo(string idx, string title, string url, string desc, DateTime date, bool favor, bool read)
        {
            RSS_idx = idx;
            Item_title = title;
            Item_url = url;
            Item_desc = desc;
            Item_date = date;
            isFavor = favor;
            isRead = read;
        }
    }
}
