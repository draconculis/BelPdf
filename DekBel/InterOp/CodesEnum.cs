using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dek.Bel.InterOp
{
    public enum CodesEnum
    {
        DEKBELCODE_SHOWBEL = 9000,
        DEKBELCODE_ADDVOLUMETITLE = 9100,
        DEKBELCODE_ADDBOOKTITLE = 9110,
        DEKBELCODE_ADDCHAPTER = 9120,
        DEKBELCODE_ADDSUBCHAPTER = 9130,
        DEKBELCODE_ADDPARAGRAPH = 9140,
        DEKBELCODE_ADDRAWCITATION = 9200,
        DEKBELCODE_ADDANDSHOWCITATION = 9300,
        DEKBELCODE_ADDCITATIONSILENT = 9301,
        DEKBELCODE_EDITCITATION = 9310,
        DEKBELCODE_STARTAUTOPAGINATION = 9400,
    }
}
