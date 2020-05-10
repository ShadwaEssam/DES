using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DES
{
    class MainClass
    {
        //for key
        static int[] pc1 = new int[] { 57, 49, 41, 33, 25, 17, 9,
                     1, 58, 50, 42, 34, 26, 18,
                     10, 2, 59, 51, 43, 35, 27,
                     19, 11, 3, 60, 52, 44, 36,
                     63, 55, 47, 39, 31, 23, 15,
                     7, 62, 54, 46, 38, 30, 22,
                     14, 6, 61, 53, 45, 37, 29,
                     21, 13, 5, 28, 20, 12, 4 };

        static int[] pc2 = new int[] { 14, 17, 11, 24, 1, 5,
                         3, 28, 15, 6, 21, 10,
                         23, 19, 12, 4, 26, 8,
                         16, 7, 27, 20, 13, 2,
                         41, 52, 31, 37, 47, 55,
                         30, 40, 51, 45, 33, 48,
                         44, 49, 39, 56, 34, 53,
                         46, 42, 50, 36, 29, 32 };


        static int[] round_table = new int[]{ 1, 1, 2, 2,
                            2, 2, 2, 2,
                            1, 2, 2, 2,
                            2, 2, 2, 1 };

        //for plaintext

        //Initial Permutation Table * 
        private static int[] ip = new int[] { 58,50,42,34,26,18,10,2,60,52,44,36,28,20,12,4,
                                       62,54,46,38,30,22,14,6,64,56,48,40,32,24,16,8,
                                       57,49,41,33,25,17,9,1,59,51,43,35,27,19,11,3,
                                       61,53,45,37,29,21,13,5,63,55,47,39,31,23,15,7 };



        //Expansion Permutation Table  *
        private static int[] ept = new int[] { 32,1,2,3,4,5,4,5,6,7,8,9,
                                        8,9,10,11,12,13,12,13,14,15,16,17,
                                        16,17,18,19,20,21,20,21,22,23,24,25,
                                        24,25,26,27,28,29,28,29,30,31,32,1 };

        private static int[] P = new int[]
                                                    {16 ,7,   20,  21 , 29 , 12 , 28,  17,
                                                        1,   15 , 23 , 26  ,5 ,  18,  31 , 10,
                                                        2,   8,   24 , 14 , 32 , 27  ,3  , 9
                                                        ,19,  13,  30,  6,   22 , 11 , 4  , 25
                                                    };


        //region SBoxes definition
       static int[,,] Sbox = new int [8,4,16] {{{14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7},
                     {0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8},
                     {4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0},
                     {15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13} },

                       {{15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10},
                       {3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5},
                       {0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15},
                       {13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9}},

                       {{10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8},
                       {13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1},
                       {13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7},
                       {1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12}},

                       {{7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15},
                       {13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9},
                       {10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4},
                       {3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14}},

                      {{2,12,4,1,7,10,11,6,8,5,3,15,13,0,14,9},
                      {14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6},
                      {4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14},
                      {11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3}},

                      {{12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11},
                      {10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8},
                      {9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6},
                      {4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,13}},

                       {{4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1},
                       {13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6},
                       {1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2},
                       {6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12}},

                       {{13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7},
                       {1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2},
                       {7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8},
                       {2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11}}};



        //Final Permutation Table * 
        private static int[] fp = new int[] { 40,8,48,16,56,24,64,32,39,7,47,15,55,23,63,31,
                                       38,6,46,14,54,22,62,30,37,5,45,13,53,21,61,29,
                                       36,4,44,12,52,20,60,28,35,3,43,11,51,19,59,27,
                                       34,2,42,10,50,18,58,26,33,1,41,9,49,17,57,25 };




        public static void Main(string[] args)
        {


            // string mainPlain = "0x0123456789ABCDEF";
            // string mainCipher = "0x85E813540F0AB405";
            //string mainKey = "0x133457799BBCDFF1";

            //triple test

            string mainPlain = "0x0123456789ABCDEF";
            
              string mainCipherTriple = "0x85E813540F0AB405";
            
            List<string> mainKeyTriple = new List<string>() { "0x133457799BBCDFF1", "0x133457799BBCDFF1" };



            //string mainPlain = "0x596F7572206C6970";
            //string mainCipher2 = "0xC0999FDDE378D7ED";
            // string mainKey = "0x0E329232EA6D0D73";

            // string mainPlain = "0x6D6573736167652E";
            // string newCipher = "0x7CF45E129445D451";
            // string mainKey = "0x38627974656B6579";


            // Encrypt(mainPlain, mainKey);
            TripEncrypt(mainPlain,mainKeyTriple);
            //TripDecrypt(mainCipherTriple, mainKeyTriple);
        }

        public static string Decrypt(string cipherText, string key)
        {
            // throw new NotImplementedException();
            return "0x0123456789ABCDEF";
        }

        public static string hexToBin(string hexaString)
        {
            var converter = new Dictionary<char, string>{
            { '0', "0000"},
            { '1', "0001"},
            { '2', "0010"},
            { '3', "0011"},

            { '4', "0100"},
            { '5', "0101"},
            { '6', "0110"},
            { '7', "0111"},

            { '8', "1000"},
            { '9', "1001"},
            { 'A', "1010"},
            { 'B', "1011"},

            { 'C', "1100"},
            { 'D', "1101"},
            { 'E', "1110"},
            { 'F', "1111"}};

            string result = "";
            for(int i=0; i<hexaString.Length; i++)
            {
                hexaString = hexaString.Replace("0x", "");
                result += converter[hexaString[i]];
            }
            return result;

        }

        public static string binToHexa(string binString)
        {
            var converter = new Dictionary<string, char>{
            {"0000", '0'},
            {"0001", '1'},
            { "0010",'2'},
            { "0011", '3'},

            { "0100", '4'},
            { "0101",'5'},
            { "0110", '6'},
            {"0111", '7'},

            { "1000", '8'},
            { "1001", '9'},
            { "1010", 'A'},
            { "1011",'B'},

            { "1100",'C'},
            {"1101", 'D'},
            {"1110", 'E'},
            {"1111", 'F'}};

            string result = "";
            binString = binString.Replace("0x", "");

            for (int i = 0; i < binString.Length; i+=4)
            {

                result += converter[binString.Substring(i, 4)];
            }
            return result;

        }

        public static string Encrypt(string plainText, string key)
        {
            // throw new NotImplementedException();

            //not working
            // string binPlain= Convert.ToString(Convert.ToInt64(plainText, 16), 2).PadLeft(64,'0');
            // string binKey=Convert.ToString(Convert.ToInt64(key, 16), 2).PadLeft(64,'0');

            string binPlain =hexToBin(plainText);
            string binKey=hexToBin(key);
            List<string> readyKeys = keyGeneration(binKey, pc1, round_table, pc2);
            String result= encryptPlain(binPlain, readyKeys, ip, fp, ept);
            // result= Convert.ToString(Convert.ToInt64(result, 2), 16);
            result= binToHexa(result);
          string concatResult= string.Concat("0x", result);


          //  Console.WriteLine(concatResult);
            return concatResult;
         



        }

        //rotate left for bits
        //will be called twice on left and right
        //take string and return 16 round
        public static List<string> performRounds(List<char> tobeRoundedList, int[] roundtable)
        {

            List<string> resultList = new List<string>();
            for (int i = 0; i < roundtable.GetLength(0); i++)
            {
                if (roundtable[i] == 2) // shift 2 or 1 bit
                {

                    char entry1 = tobeRoundedList[0];
                    char entry2 = tobeRoundedList[1];
                    tobeRoundedList.RemoveAt(1);
                    tobeRoundedList.RemoveAt(0);
                    tobeRoundedList.Add(entry1);
                    tobeRoundedList.Add(entry2);

                }
                else
                {
                    char entry1 = tobeRoundedList[0];
                    tobeRoundedList.RemoveAt(0);
                    tobeRoundedList.Add(entry1);

                }
                string x = new string(tobeRoundedList.ToArray());
                resultList.Add(x);

            }
            return resultList;

        }
        //hnmshy 3l array nakhod l trteb l by2ol 3leh w format xx based on it 
        public static string permuteTable(string xx, int[] arr)
        {
            string result = ""; //
            for (int i = 0; i < arr.Length; i++)
            {
                result += xx[arr[i] - 1];
            }
            return result; //
        }


        public static List<string> keyGeneration(string key, int[] pc1, int[] roundTable, int[] pc2)
        {
            //call permutation table
            //permuted key 58 bits
            string permutedKey = permuteTable(key, pc1);
            string initialLeftKey = permutedKey.Substring(0, 28);
            string initialRightKey = permutedKey.Substring(28, 28);

            List<char> ilk = initialLeftKey.ToList();

            List<char> irk = initialRightKey.ToList();

            List<string> lstLeft = performRounds(ilk, round_table);
            List<string> lstRight = performRounds(irk, round_table);

            List<string> keys = new List<string>();
            for (int i = 0; i < lstLeft.Count; i++)
            {
                keys.Add(permuteTable(lstLeft[i] + lstRight[i], pc2));
            }
            return keys;
        }


        public static string encryptPlain(string plainText, List<string> keys, int[] IP, int [] FP, int [] expTable)
        {
            string permutedPlain= permuteTable(plainText,IP);
           
            String[] L = new String[17];
            String[] R = new String[17];
            L[0]=permutedPlain.Substring(0, 32);
            R[0]=permutedPlain.Substring(32, 32);
            //loop 16 times getting 16 Ls, Rs
            //concat R16L16
            //permute by FP

            for (int i = 1; i <= 16; i++)
            {
                // Equations Ln= Rn-1
                //           Rn= Ln-1+ f(Rn-1,Kn)
                L[i] = R[i-1];
                R[i] = XOR(L[i - 1], F(R[i - 1], keys[i-1], expTable));
               

            } 
            String result = permuteTable(string.Concat(R[16], L[16]), FP);
            return result;


        }
        public static string F(string RnPrev, string Kn, int[]ept)
        {
            // extend Rn to 48 by exp table

            //xor step

            //put result in sbox to be 32 bits

            //return sbox(XOR(permuteTable(RnPrev, ept), Kn));
            return permuteTable(sbox(XOR(permuteTable(RnPrev, ept), Kn)), P);

        }

        public static string XOR(string first, string second)
        {
            string result = "";
            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] == second[i])
                
                    result += "0";
                
                else
                    result += "1";
            }
            return result; 
        }
        //return 32 bit string given 48bit string
        public static string sbox(string x)
        {
            string result = "";
            string str = "";
            
            for (int i = 0; i < x.Length; i += 6)
            {
               str= x.Substring(i, 6);
                //to convert binary string to dec 
               int I = Convert.ToInt32(string.Concat(str[0] ,str[5]), 2);
               int J = Convert.ToInt32(str.Substring(1, 4), 2);
               string entry = Convert.ToString(Sbox[i/6,I, J], 2).PadLeft(4,'0');
               result += entry;

            }
           return result; 

        }



        // TRIPLE

        public static string TripEncrypt(string plainText, List<string> key)
        {
            //throw new NotImplementedException();
            //we have 3 keys
            //encrypt with 1st
            string one= Encrypt(plainText,key[0]); 
            //decrypt with 2nd
             string two=Decrypt(one, key[1]); 
            //encrypt with 3rd
            string three = Encrypt(two, key[0]);
            Console.WriteLine(three);
            return three;

        }

        public static string TripDecrypt(string cipherText, List<string> key)
        {
            
            //decrypt with 1st
            string one = Decrypt(cipherText, key[0]);
            //enc with 2nd
            string two = Encrypt(one, key[1]);
            //dec with 3rd
            string three = Decrypt(two, key[0]);
            Console.WriteLine(three);
            return three;

        }

        public List<string> Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }



    }
}



