using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HomeworkATM
{
    public class Card
    {
        public string cardNumber { get; private set; }
        public string ownerName { get; private set; }
        public string endDate { get; private set; }
        public string issuingBank { get; private set; }
        public double moneySum { get; set; }

        public Card(string cardNumber, string ownerName, string endDate, string issuingBank, double moneySum)
        {
            if (cardNumber.Length == 16 && Regex.IsMatch(cardNumber, @"[0-9]{16}"))
            {
                this.cardNumber = cardNumber;
            }
            else
            {
                throw new Exception();
            }

            this.ownerName = ownerName;
            if (Regex.IsMatch(endDate, @"(0[1-9]|1[0-2])\.(0[1-9]|[1-9][0-9])"))
            {
                this.endDate = endDate;
            }
            this.issuingBank = issuingBank;
            this.moneySum = moneySum;
        }


        public string ToString()
        {
            return $"Номер карты: {cardNumber} \nИмя владельца: {ownerName} \nМесяц и год окончания действия карты: {endDate} \nБанк-эмитент карты: {issuingBank} \nСумма денег на счету: {moneySum}\n";
        }

    }

}
