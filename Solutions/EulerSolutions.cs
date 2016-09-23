namespace EulerSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ProjectEuler;
   
    /// <summary>
    /// Summary description for EulerSolutions
    /// </summary>
    [TestClass]
    public class EulerSolutions
    {
        public EulerSolutions()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /* Generate a sequence of lottery numbers */
        [TestMethod]
        public void Problem0()
        {
            // Orderby Guid?!  Crazy... How does that work?
            Enumerable.Range(1, 49).OrderBy(t => Guid.NewGuid()).Take(6);
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public public void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public public void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /* If we list all the natural numbers below 10 that are multiples of 3 or 5, we get 3, 5, 6 and 9. 
         * The sum of these multiples is 23.
           Find the sum of all the multiples of 3 or 5 below 1000.*/
        [TestMethod]
        public void Problem1()
        {
            var result = Enumerable.Range(1, 999).Where(i => i % 3 == 0 || i % 5 == 0).Sum();

            if (result != 233168)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /*
         By considering the terms in the Fibonacci sequence whose values do not exceed four million, 
         * find the sum of the even-valued terms.
         */
        [TestMethod]
        public void Problem2()
        {
            var result = Sequences.Fibbonaci()
                .TakeWhile(v => v < 4000000)
                .Where(v => v % 2 == 0)
                .Sum();

            if (result != 4613732)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /*
         The prime factors of 13195 are 5, 7, 13 and 29.
         What is the largest prime factor of the number 600851475143 ?
         */
        [TestMethod]
        public void Problem3()
        {
            long number = 600851475143;
            var values = Enumerable.Range(2, (int)Math.Sqrt(number)).Where(n => number % n == 0 && n.IsPrime());
            int answer = values.Last();

            if (answer != 6857)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* A palindromic number reads the same both ways. 
         * The largest palindrome made from the product of two 2-digit numbers is 9009 = 91  99.
         * Find the largest palindrome made from the product of two 3-digit numbers.
        */
        [TestMethod]
        public void Problem4()
        {
            // This is funky...
            var numbers = Enumerable.Range(1, 999).Reverse();

            var largestPalendrome =
                Enumerable.Range(99 * 99, 999 * 999).Reverse()
                    .Where(n => n.IsPalendrome())
                    .Where(n => numbers.Any(v => n % v == 0 && n / v < numbers.Max()))
                    .First();

            if (largestPalendrome != 906609)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* What is the smallest number divisible by each of the numbers 1 to 20? */
        [TestMethod]
        public void Problem5()
        {
            // We know that we can take the largest prime factorisation for each number to assemble the lowest divisible number.
            var number = Enumerable.Range(1, 20)
                .SelectMany(v => PrimeGenerator.PrimeFactors(v))
                .GroupBy(l => l.Key)
                .Select(g => g.OrderByDescending(i => i.Value).First())
                .Aggregate(1, (h, t) => h * (int)Math.Pow(t.Key, t.Value));

            // Brute force...
            //var number = Enumerable.Range(20, int.MaxValue - 20)
            //                       .AsParallel()
            //                       .First(v => Enumerable.Range(1, 20).All(r => v % r == 0));
            

            if (number != 232792560)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Find the difference between the sum of the squares of the first one hundred natural numbers and the square of the sum. */
        [TestMethod]
        public void Problem6()
        {
            var sumOfSquares = Enumerable.Range(1, 100).Select(n => n * n).Sum();
            var squareOfSum = (int)Math.Pow(Enumerable.Range(1, 100).Sum(), 2.0);
            var answer = squareOfSum - sumOfSquares;

            if (answer != 25164150)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }


        /* Find the 10001st prime. */
        [TestMethod]
        public void Problem7()
        {
            var prime = Sequences.Primes().ElementAt(10001 - 1);

            if (prime != 104743)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Find the greatest product of five consecutive digits in the 1000-digit number. */
        [TestMethod]
        public void Problem8()
        {
            string OneThousandDigitNumber =
                @"7316717653133062491922511967442657474235534919493496983520312774506326239578318016984801869478851843858615607891129494954595017379583319528532088055111254069874715852386305071569329096329522744304355766896648950445244523161731856403098711121722383113622298934233803081353362766142828064444866452387493035890729629049156044077239071381051585930796086670172427121883998797908792274921901699720888093776657273330010533678812202354218097512545405947522435258490771167055601360483958644670632441572215539753697817977846174064955149290862569321978468622482839722413756570560574902614079729686524145351004748216637048440319989000889524345065854122758866688116427171479924442928230863465674813919123162824586178664583591245665294765456828489128831426076900422421902267105562632111110937054421750694165896040807198403850962455444362981230987879927244284909188845801561660979191338754992005240636899125607176060588611646710940507754100225698315520005593572972571636269561882670428252483600823257530420752963450";
            int consecutiveDigits = 5;

            var result = Enumerable.Range(0, OneThousandDigitNumber.Length - consecutiveDigits)
                .Select(start => int.Parse(OneThousandDigitNumber.Substring(start, consecutiveDigits))
                .GetDigitsYield()
                .Aggregate((h, t) => h * t))
                .Max();

            if (result != 40824)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* There exists exactly one Pythagorean triplet for which a + b + c = 1000.
        Find the product abc. */
        [TestMethod]
        public void Problem9()
        {
            var answer = Sequences.PythagoreanTriplets(1000).First(t => t.Item1 + t.Item2 + t.Item3 == 1000);
            if (answer.Item1 * answer.Item2 * answer.Item3 != 31875000)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Calculate the sum of all the primes below two million. */
        [TestMethod]
        public void Problem10()
        {
            long sum = Sequences.Primes().TakeWhile(p => p < 2000000).Sum(x => (long)x);

            if (sum != 142913828922)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* What is the greatest product of four adjacent numbers in any direction (up, down, left, right, or diagonally) in the 2020 grid? */
        [TestMethod]
        public void Problem11()
        {
            var grid = new int[,]
                               {
                                    {08,02,22,97,38,15,00,40,00,75,04,05,07,78,52,12,50,77,91,08},
                                    {49,49,99,40,17,81,18,57,60,87,17,40,98,43,69,48,04,56,62,00},
                                    {81,49,31,73,55,79,14,29,93,71,40,67,53,88,30,03,49,13,36,65},
                                    {52,70,95,23,04,60,11,42,69,24,68,56,01,32,56,71,37,02,36,91},
                                    {22,31,16,71,51,67,63,89,41,92,36,54,22,40,40,28,66,33,13,80},
                                    {24,47,32,60,99,03,45,02,44,75,33,53,78,36,84,20,35,17,12,50},
                                    {32,98,81,28,64,23,67,10,26,38,40,67,59,54,70,66,18,38,64,70},
                                    {67,26,20,68,02,62,12,20,95,63,94,39,63,08,40,91,66,49,94,21},
                                    {24,55,58,05,66,73,99,26,97,17,78,78,96,83,14,88,34,89,63,72},
                                    {21,36,23,09,75,00,76,44,20,45,35,14,00,61,33,97,34,31,33,95},
                                    {78,17,53,28,22,75,31,67,15,94,03,80,04,62,16,14,09,53,56,92},
                                    {16,39,05,42,96,35,31,47,55,58,88,24,00,17,54,24,36,29,85,57},
                                    {86,56,00,48,35,71,89,07,05,44,44,37,44,60,21,58,51,54,17,58},
                                    {19,80,81,68,05,94,47,69,28,73,92,13,86,52,17,77,04,89,55,40},
                                    {04,52,08,83,97,35,99,16,07,97,57,32,16,26,26,79,33,27,98,66},
                                    {88,36,68,87,57,62,20,72,03,46,33,67,46,55,12,32,63,93,53,69},
                                    {04,42,16,73,38,25,39,11,24,94,72,18,08,46,29,32,40,62,76,36},
                                    {20,69,36,41,72,30,23,88,34,62,99,69,82,67,59,85,74,04,36,16},
                                    {20,73,35,29,78,31,90,01,74,31,49,71,48,86,81,16,23,57,05,54},
                                    {01,70,54,71,83,51,54,69,16,92,33,48,61,43,52,01,89,19,67,48}
                               };

            var maxProduct = grid.AllAdjacentNumbers().Max(array => array.Aggregate((h,t) => h * t));

            if (maxProduct != 70600674)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* What is the value of the first triangle number to have over five hundred divisors? */
        [TestMethod]
        public void Problem12()
        {
            long result = Sequences.TriangleNumbers()
                                   .First(tn => tn.NumberOfDivisors() > 500);

            if (result != 76576500)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Find the first ten digits of the sum of one-hundred 50-digit numbers. */
        [TestMethod]
        public void Problem13()
        {
            var value = new List<BigNumber>
                             {
                                 "37107287533902102798797998220837590246510135740250",
                                 "46376937677490009712648124896970078050417018260538",
                                 "74324986199524741059474233309513058123726617309629",
                                 "91942213363574161572522430563301811072406154908250",
                                 "23067588207539346171171980310421047513778063246676",
                                 "89261670696623633820136378418383684178734361726757",
                                 "28112879812849979408065481931592621691275889832738",
                                 "44274228917432520321923589422876796487670272189318",
                                 "47451445736001306439091167216856844588711603153276",
                                 "70386486105843025439939619828917593665686757934951",
                                 "62176457141856560629502157223196586755079324193331",
                                 "64906352462741904929101432445813822663347944758178",
                                 "92575867718337217661963751590579239728245598838407",
                                 "58203565325359399008402633568948830189458628227828",
                                 "80181199384826282014278194139940567587151170094390",
                                 "35398664372827112653829987240784473053190104293586",
                                 "86515506006295864861532075273371959191420517255829",
                                 "71693888707715466499115593487603532921714970056938",
                                 "54370070576826684624621495650076471787294438377604",
                                 "53282654108756828443191190634694037855217779295145",
                                 "36123272525000296071075082563815656710885258350721",
                                 "45876576172410976447339110607218265236877223636045",
                                 "17423706905851860660448207621209813287860733969412",
                                 "81142660418086830619328460811191061556940512689692",
                                 "51934325451728388641918047049293215058642563049483",
                                 "62467221648435076201727918039944693004732956340691",
                                 "15732444386908125794514089057706229429197107928209",
                                 "55037687525678773091862540744969844508330393682126",
                                 "18336384825330154686196124348767681297534375946515",
                                 "80386287592878490201521685554828717201219257766954",
                                 "78182833757993103614740356856449095527097864797581",
                                 "16726320100436897842553539920931837441497806860984",
                                 "48403098129077791799088218795327364475675590848030",
                                 "87086987551392711854517078544161852424320693150332",
                                 "59959406895756536782107074926966537676326235447210",
                                 "69793950679652694742597709739166693763042633987085",
                                 "41052684708299085211399427365734116182760315001271",
                                 "65378607361501080857009149939512557028198746004375",
                                 "35829035317434717326932123578154982629742552737307",
                                 "94953759765105305946966067683156574377167401875275",
                                 "88902802571733229619176668713819931811048770190271",
                                 "25267680276078003013678680992525463401061632866526",
                                 "36270218540497705585629946580636237993140746255962",
                                 "24074486908231174977792365466257246923322810917141",
                                 "91430288197103288597806669760892938638285025333403",
                                 "34413065578016127815921815005561868836468420090470",
                                 "23053081172816430487623791969842487255036638784583",
                                 "11487696932154902810424020138335124462181441773470",
                                 "63783299490636259666498587618221225225512486764533",
                                 "67720186971698544312419572409913959008952310058822",
                                 "95548255300263520781532296796249481641953868218774",
                                 "76085327132285723110424803456124867697064507995236",
                                 "37774242535411291684276865538926205024910326572967",
                                 "23701913275725675285653248258265463092207058596522",
                                 "29798860272258331913126375147341994889534765745501",
                                 "18495701454879288984856827726077713721403798879715",
                                 "38298203783031473527721580348144513491373226651381",
                                 "34829543829199918180278916522431027392251122869539",
                                 "40957953066405232632538044100059654939159879593635",
                                 "29746152185502371307642255121183693803580388584903",
                                 "41698116222072977186158236678424689157993532961922",
                                 "62467957194401269043877107275048102390895523597457",
                                 "23189706772547915061505504953922979530901129967519",
                                 "86188088225875314529584099251203829009407770775672",
                                 "11306739708304724483816533873502340845647058077308",
                                 "82959174767140363198008187129011875491310547126581",
                                 "97623331044818386269515456334926366572897563400500",
                                 "42846280183517070527831839425882145521227251250327",
                                 "55121603546981200581762165212827652751691296897789",
                                 "32238195734329339946437501907836945765883352399886",
                                 "75506164965184775180738168837861091527357929701337",
                                 "62177842752192623401942399639168044983993173312731",
                                 "32924185707147349566916674687634660915035914677504",
                                 "99518671430235219628894890102423325116913619626622",
                                 "73267460800591547471830798392868535206946944540724",
                                 "76841822524674417161514036427982273348055556214818",
                                 "97142617910342598647204516893989422179826088076852",
                                 "87783646182799346313767754307809363333018982642090",
                                 "10848802521674670883215120185883543223812876952786",
                                 "71329612474782464538636993009049310363619763878039",
                                 "62184073572399794223406235393808339651327408011116",
                                 "66627891981488087797941876876144230030984490851411",
                                 "60661826293682836764744779239180335110989069790714",
                                 "85786944089552990653640447425576083659976645795096",
                                 "66024396409905389607120198219976047599490197230297",
                                 "64913982680032973156037120041377903785566085089252",
                                 "16730939319872750275468906903707539413042652315011",
                                 "94809377245048795150954100921645863754710598436791",
                                 "78639167021187492431995700641917969777599028300699",
                                 "15368713711936614952811305876380278410754449733078",
                                 "40789923115535562561142322423255033685442488917353",
                                 "44889911501440648020369068063960672322193204149535",
                                 "41503128880339536053299340368006977710650566631954",
                                 "81234880673210146739058568557934581403627822703280",
                                 "82616570773948327592232845941706525094512325230608",
                                 "22918802058777319719839450180888072429661980811197",
                                 "77158542502016545090413245809786882778948721859617",
                                 "72107838435069186155435662884062257473692284509516",
                                 "20849603980134001723930671666823555245252804609722",
                                 "53503534226472524250874054075591789781264330331690",
                             }.BigSum();

            if (value != "5537376230390876637302048746832985971773659831892672")
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        [TestMethod]
        public void Problem14()
        {
            var highestValue = Enumerable.Range(1, 1000000).Select(v => Tuple.Create(v, v.CollatzChainLength())).OrderByDescending(v => v.Item2).First();

            if (highestValue.Item1 != 837799)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Starting in the top left corner of a 22 grid, there are 6 routes (without backtracking) to the bottom right corner.
           How many routes are there through a 2020 grid? */
        [TestMethod]
        public void Problem15()
        {
            int size = 20;
            var val =  Sequences.GetPascalTriangle().Where(a => a.Length % 2 == 1).Select(r => r.Max()).Take(size + 1).Last();

            if (val != 137846528820)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* What is the sum of the digits of the number 2^1000? */
        [TestMethod]
        public void Problem16()
        {
            var total = ((BigNumber)2 ^ 1000L).Digits().Sum();
            if (total != 1366)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* If the numbers 1 to 5 are written out in words: one, two, three, four, five, 
         * then there are 3 + 3 + 5 + 4 + 4 = 19 letters used in total.
         * If all the numbers from 1 to 1000 (one thousand) inclusive were written out in words, how many letters would be used? 
         * NOTE: Do not count spaces or hyphens. */
        public void Problem17()
        {
            int value = Enumerable.Range(1, 1000).Select(n => n.ToWords().Count(c => c != ' ' && c != '-')).Sum();

            if (value != 21124)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* By starting at the top of the triangle below and moving to adjacent numbers on the row below, the maximum total from top to bottom is 23.

                        3
                        7 4
                        2 4 6
                        8 5 9 3

            That is, 3 + 7 + 4 + 9 = 23.*/
        public void Problem18()
        {
            int[][] triangle = new int[][] { 
                                                        new[] {75}, 
                                                        new[] {95, 64},
                                                        new[] {17, 47, 82},
                                                        new[] {18, 35, 87, 10},
                                                        new[] {20, 04, 82, 47, 65},
                                                        new[] {19, 01, 23, 75, 03, 34},
                                                        new[] {88, 02, 77, 73, 07, 63, 67},
                                                        new[] {99, 65, 04, 28, 06, 16, 70, 92},
                                                        new[] {41, 41, 26, 56, 83, 40, 80, 70, 33},
                                                        new[] {41, 48, 72, 33, 47, 32, 37, 16, 94, 29},
                                                        new[] {53, 71, 44, 65, 25, 43, 91, 52, 97, 51, 14},
                                                        new[] {70, 11, 33, 28, 77, 73, 17, 78, 39, 68, 17, 57},
                                                        new[] {91, 71, 52, 38, 17, 14, 91, 43, 58, 50, 27, 29, 48},
                                                        new[] {63, 66, 04, 68, 89, 53, 67, 30, 73, 16, 69, 87, 40, 31},
                                                        new[] {04, 62, 98, 27, 23, 09, 70, 98, 73, 93, 38, 53, 60, 04, 23 }  };

            var maxPath = triangle.CollapseTree(); 

            if (maxPath != 1074)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* 
         You are given the following information, but you may prefer to do some research for yourself.

        1 Jan 1900 was a Monday. 
        Thirty days has September,
        April, June and November.
        All the rest have thirty-one,
        Saving February alone,
        Which has twenty-eight, rain or shine.
        And on leap years, twenty-nine. 
        
        A leap year occurs on any year evenly divisible by 4, but not on a century unless it is divisible by 400. 
        How many Sundays fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000)?

        */
        [TestMethod]
        public void Problem19()
        {
            DateTime start = new DateTime(1901, 1, 1);
            DateTime end = new DateTime(2000, 12, 31);

            var months = start.MonthsUntil(end).Where(d => d.DayOfWeek == DayOfWeek.Sunday).Count();
            if (months != 171)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Find the sum of digits in 100! */
        [TestMethod]
        public void Problem20()
        {
            var value = ((BigNumber)100).Factorial().Digits().Sum();

            if (value != 648)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /*
            Let d(n) be defined as the sum of proper divisors of n (numbers less than n which divide evenly into n).
            If d(a) = b and d(b) = a, where a  b, then a and b are an amicable pair and each of a and b are called amicable numbers.
            For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110; therefore d(220) = 284. 
            The proper divisors of 284 are 1, 2, 4, 71 and 142; so d(284) = 220.
            Evaluate the sum of all the amicable numbers under 10000.
        */

        [TestMethod]
        public void Problem21()
        {
            var answer = Enumerable.Range(1, 10000).Where(i =>
                                                          {
                                                              var sumOfI = i.ProperDivisors().Sum();
                                                              var sumOfIProperDivisors = sumOfI.ProperDivisors().Sum();

                                                              // Amicable pairs cannot be the same... They must be a pair of different numbers whatever order.
                                                              return sumOfIProperDivisors == i && sumOfI != i;
                                                          }).Sum();

            Assert.AreEqual(31626, answer);
        }

        /* 
         * For example, when the list is sorted into alphabetical order, 
         * COLIN, which is worth 3 + 15 + 12 + 9 + 14 = 53, is the 938th name in the list. 
         * So, COLIN would obtain a score of 938  53 = 49714.
         * What is the total of all the name scores in the file? */
        [TestMethod]
        public void Problem22()
        {
            int i = 1;
            int result = FileSequences.Items(@".\Data\Names.txt", new string[] { "\",", "\"" })
                .OrderBy(n => n)
                .Select(n => n.Select(l => (int)l - 64).Sum() * i++)
                .Sum();

            if (result != 871198282)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /*
         * A perfect number is a number for which the sum of its proper divisors is exactly equal to the number. 
         * For example, the sum of the proper divisors of 28 would be 1 + 2 + 4 + 7 + 14 = 28, which means that 28 is a perfect number.
         * A number n is called deficient if the sum of its proper divisors is less than n and it is called abundant if this sum exceeds n.
         * As 12 is the smallest abundant number, 1 + 2 + 3 + 4 + 6 = 16, the smallest number that can be written as the sum of two abundant numbers is 24. 
         * By mathematical analysis, it can be shown that all integers greater than 28123 can be written as the sum of two abundant numbers. 
         * However, this upper limit cannot be reduced any further by analysis even though it is known that the greatest number that cannot be 
         * expressed as the sum of two abundant numbers is less than this limit.
         * Find the sum of all the positive integers which cannot be written as the sum of two abundant numbers.
         */
        [TestMethod]
        public void Problem23()
        {
            var abundant = new HashSet<int>();
            var notsumofabundant = new List<int>();

            for (int i = 1; i <= 28123; i++)
            {
                if (i.IsAbundant())
                {
                    abundant.Add(i);
                }

                bool sumofabundant = abundant.Select(a => i - a).Any(abundant.Contains);

                if (!sumofabundant)
                {
                    notsumofabundant.Add(i);
                }
            }

            var answer = notsumofabundant.Sum();

            if (answer != 4179871)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* A permutation is an ordered arrangement of objects. 
         * For example, 3124 is one possible permutation of the digits 1, 2, 3 and 4. 
         * If all of the permutations are listed numerically or alphabetically, we call it lexicographic order. 
         * The lexicographic permutations of 0, 1 and 2 are:
             012   021   102   120   201   210
             What is the millionth lexicographic permutation of the digits 0, 1, 2, 3, 4, 5, 6, 7, 8 and 9?
        */
        [TestMethod]
        public void Problem24()
        {
            var array = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var allPermutations = array.Permute();

            var millionthPermutation = allPermutations.Take(1000000).Last();

            if (millionthPermutation.ToArray().GetNumber() != 2783915460)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* What is the first term in the Fibonacci sequence to contain 1000 digits? */
        [TestMethod]
        public void Problem25()
        {
            var digits = Sequences.BigFibbonaci().TakeWhile(b => b.Digits().Count() != 1000).Count() + 1;

            if (digits != 4782)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* A unit fraction contains 1 in the numerator. The decimal representation of the unit fractions with denominators 2 to 10 are given:
             1/2 = 0.5
             1/3 = 0.(3)
             1/4 = 0.25
             1/5 = 0.2
             1/6 = 0.1(6)
             1/7 = 0.(142857)
             
         * Where 0.1(6) means 0.166666..., and has a 1-digit recurring cycle. It can be seen that 1/7 has a 6-digit recurring cycle.
         * Find the value of d 1000 for which 1/d contains the longest recurring cycle in its decimal fraction part. */
        [TestMethod]
        public void Problem26()
        {
            var sequences = Enumerable.Range(1, 1000)
                                      .Select(n => Tuple.Create(n, Sequences.DivisorSequence(1, n)))
                                      .OrderByDescending(s => s.Item2.Length)
                                      .ToArray();

            var longestCycle = sequences.First().Item1;

            if(longestCycle != 983)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        [TestMethod]
        [Ignore]
        public void Problem27()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var p = PrimeGenerator.Primes().Take(200000).ToArray();
            stopWatch.Stop();

            var stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            var p2 = Sequences.Primes().Take(200000).ToArray();
            stopWatch2.Stop();

            CollectionAssert.AreEquivalent(p, p2);
            Assert.IsTrue(stopWatch.Elapsed < stopWatch2.Elapsed);
        }

        /* Starting with the number 1 and moving to the right in a clockwise direction a 5 by 5 spiral is formed as follows:
           
                 21 22 23 24 25
                 20  7  8  9 10
                 19  6  1  2 11
                 18  5  4  3 12
                 17 16 15 14 13
 
           It can be verified that the sum of the numbers on the diagonals is 101.
           What is the sum of the numbers on the diagonals in a 1001 by 1001 spiral formed in the same way?*/
        [TestMethod]
        public void Problem28()
        {
            int spiralMaxSize = 1001;
            int currentValue = 1;

            List<int> corners = new List<int>();
            List<int> spirals = new List<int>();

            // Add one.
            corners.Add(currentValue);
            spirals.Add(1);

            for (int i = 3; i <= spiralMaxSize; i += 2)
            {
                int skipValue = spirals.Last() + 1;
                int runLength = (int)Math.Pow(i, 2.0);

                for (int j = currentValue + skipValue; j <= runLength; j += skipValue)
                {
                    corners.Add(j);
                    currentValue = j;
                }

                spirals.Add(i);
            }

            if (669171001 != corners.Sum())
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* How many distinct terms are in the sequence generated by ab for 2  a  100 and 2  b  100? */
        public void Problem29()
        {
            int max = 100;

            var numbers = new List<BigNumber>();

            for (int i = 2; i <= max; i++)
            {
                for (int j = 2; j <= max; j++)
                {
                    numbers.Add((BigNumber)i ^ (BigNumber)j);
                }
            }

            int answer = numbers.Distinct().Count();
            if (answer != 9183)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Find the sum of all the numbers that can be written as the sum of fifth powers of their digits.*/
        [TestMethod]
        public void Problem30()
        {
            var answer = Enumerable.Range(2, 1000000)
                .Where(n => n == n.GetDigitsYield()
                              .Select(d => (int) Math.Pow(d, 5.0))
                              .Sum())
                .Sum();

            if (answer != 443839)
            {
               throw new InvalidOperationException("Problem not solved correctly"); 
            }
        }

        /*  In England the currency is made up of pound, £, and pence, p, and there are eight coins in general circulation:

            1p, 2p, 5p, 10p, 20p, 50p, £1 (100p) and £2 (200p).
            It is possible to make £2 in the following way:

            1£1 + 150p + 220p + 15p + 12p + 31p
            How many different ways can £2 be made using any number of coins?*/
        [TestMethod]
        public void Problem31()
        {
            //// Ugly, ugly brute force.
            //int combinations = 0;
            //for (int p200 = 0; p200 <= 1; p200++)
            //{
            //    for (int p100 = 0; p100 <= 2; p100++)
            //    {
            //        for (int p50 = 0; p50 <= 4; p50++)
            //        {
            //            for (int p20 = 0; p20 <= 10; p20++)
            //            {
            //                for (int p10 = 0; p10 <= 20; p10++)
            //                {
            //                    for (int p5 = 0; p5 <= 40; p5++)
            //                    {
            //                        for (int p2 = 0; p2 <= 100; p2++)
            //                        {
            //                            for (int p1 = 0; p1 <= 200; p1++)
            //                            {
            //                                if ((p200 * 200 + p100 * 100 + p50 * 50 + p20 * 20 + p10 * 10 + p5 * 5 + p2 * 2 + p1 * 1) == DesiredValue)
            //                                {
            //                                    combinations++;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}


            // Improved recursive solution.
            const int DesiredValue = 200;
            List<int> denominations = new List<int>() { 1, 2, 5, 10, 20, 50, 100, 200 };
            var combinations = denominations.SumOfCombinations(DesiredValue);
            Assert.AreEqual(73682, combinations, "Problem not solved correctly"); 
        }

        /* Find the sum of all numbers which are equal to the sum of the factorial of their digits. */
        [TestMethod]
        public void Problem34()
        {
            var answer = Enumerable.Range(3, 10000000)
                                   .Where(n => n == n.GetDigitsYield()
                                                     .Select(d => d.Factorial())
                                                     .Sum())
                                   .Sum();

            Assert.AreEqual(40730, answer, "Problem not solved correctly");
        }

        /* How many circular primes are there below one million?  */
        [TestMethod]
        public void Problem35()
        {
            var circularprimes = Sequences.Primes()
                                          .TakeWhile(p => p < 1000000)
                                          .Where(p => p.RotateDigits()
                                                       .All(r => r.IsPrime()))
                                          .Count();

            Assert.AreEqual(55, circularprimes, "Problem not solved correctly");
        }

        /*  The decimal number, 585 = 10010010012 (binary), is palindromic in both bases.
            Find the sum of all numbers, less than one million, which are palindromic in base 10 and base 2. */
        [TestMethod]
        public void Problem36()
        {
            var result = Enumerable.Range(1, 1000000).Where(n => n.IsPalendrome() && Convert.ToString(n, 2).IsPalendrome()).Sum();

            if (result != 872187)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        [TestMethod]
        public void Problem37()
        {
            var truncatable = Sequences.CachedPrimes()
                                        .Where(p => p > 7)
                                        .Where(p => p.Truncations()
                                                     .All(t => t.IsPrime()))
                                        .Take(11)
                                        .Sum();

            if (truncatable != 748317)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* If p is the perimeter of a right angle triangle with integral length sides, {a,b,c}, there are exactly three solutions for p = 120.
                {20,48,52}, {24,45,51}, {30,40,50}
           For which value of p 1000, is the number of solutions maximised? */
        [TestMethod]
        public void Problem39()
        {
            // Brute force...  If there aren't multiple cores, this will take a very long time to run.
            // Really need to fix this.
            var tripets = Enumerable
                                  .Range(500, 1000)
                                  .AsParallel()
                                  .Select(v => Tuple.Create(v, Sequences.PythagoreanTripletsPerimeter(v)))
                                  .OrderBy(t => t.Item2.Count())
                                  .ToList();

            var maxPerimeter = tripets.Last();

            if (maxPerimeter.Item1 != 840)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }   
        }

        /* An irrational decimal fraction is created by concatenating the positive integers:
           0.123456789101112131415161718192021...
           It can be seen that the 12th digit of the fractional part is 1.
           If dn represents the nth digit of the fractional part, find the value of the following expression.
           d[1]  d[10]  d[100]  d[1000]  d[10000]  d[100000]  d[1000000]
        */
        [TestMethod]
        public void Problem40()
        {
            StringBuilder sb = new StringBuilder();
            int i = 1;

            while (sb.Length < 1000000)
            {
                sb.Append(i.ToString());
                i++;
            }

            string number = sb.ToString();

            var value1 = number.ElementAt(0).ConvertToInt();
            var value2 = number.ElementAt(9).ConvertToInt();
            var value3 = number.ElementAt(99).ConvertToInt();
            var value4 = number.ElementAt(999).ConvertToInt();
            var value5 = number.ElementAt(9999).ConvertToInt();
            var value6 = number.ElementAt(99999).ConvertToInt();
            var value7 = number.ElementAt(999999).ConvertToInt();

            int answer = value1 * value2 * value3 * value4 * value5 * value6 * value7;

            if (answer != 210)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once. 
           For example, 2143 is a 4-digit pandigital and is also prime.
           What is the largest n-digit pandigital prime that exists? */
        [TestMethod]
        public void Problem41()
        {
            var array = new[] { 1, 2, 3, 4, 5, 6, 7 };

            var max = array.Permute()
                           .Select(t => t.ToArray().FromDigits())
                           .Where(v => v.IsPrime())
                           .Max();

            if (max != 7652413)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        [TestMethod]
        public void Problem42()
        {
            var words = FileSequences.Items(@".\Data\Words.txt", new string[] { "\",", "\"" });

            int triangle = words.Select(n => n.Select(l => (int)l - 64)
                                .Sum())
                                .Count(n => ((long)n).IsTriangle());

            Assert.AreEqual(162, triangle);
        }

        /* Triangle, pentagonal, and hexagonal numbers are generated by the following formulae:
           Triangle     | Tn=n(n+1)/2 | 1, 3, 6, 10, 15, ...
           Pentagonal   | Pn=n(3n1)/2 | 1, 5, 12, 22, 35, ...
           Hexagonal    | Hn=n(2n1)   |1, 6, 15, 28, 45, ...
           It can be verified that T285 = P165 = H143 = 40755.
           Find the next triangle number that is also pentagonal and hexagonal. */
        [TestMethod]
        public void Problem45()
        {
            HashSet<long> hh = new HashSet<long>(Sequences.HexagonalNumbers().Take(1000000));
            HashSet<long> hp = new HashSet<long>(Sequences.PentagonalNumbers().Take(1000000));

            hh.IntersectWith(hp);

            var matching = hh.Where(h => h.IsTriangle()).ToArray();

            if(matching.Last() != 1533776805)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Find the last ten digits of the series, 1^1 + 2^2 + 3^3 + ... + 1000^1000. */
        [TestMethod]
        public void Problem48()
        {
            // Calculate sum of exponents...
            var value = Enumerable.Range(1, 1000).AsParallel().Select(v => (BigNumber)v ^ (BigNumber)v).BigSum();

            // Get last 10 digits.
            var digits = value.Digits().Skip(value.Digits().Count() - 10).Take(10);

            if (new BigNumber(digits) != 9110846700)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /*It can be seen that the number, 125874, and its double, 251748, contain exactly the same digits, but in a different order.
          Find the smallest positive integer, x, such that 2x, 3x, 4x, 5x, and 6x, contain the same digits.*/
        [TestMethod]
        public void Problem52()
        {
            int smallest = Enumerable.Range(1, int.MaxValue)
                                    .First(val => (
                                                   !(val).GetDigitsYield().Except((val * 2).GetDigitsYield()).Any()
                                                   && !(val * 2).GetDigitsYield().Except((val * 3).GetDigitsYield()).Any()
                                                   && !(val * 3).GetDigitsYield().Except((val * 4).GetDigitsYield()).Any()
                                                   && !(val * 4).GetDigitsYield().Except((val * 5).GetDigitsYield()).Any()
                                                   && !(val * 5).GetDigitsYield().Except((val * 6).GetDigitsYield()).Any()));

            if (smallest != 142857)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        [TestMethod]
        public void Problem54()
        {
            var result = FileSequences.Items(@".\Data\Poker.txt", new string[] { "\r\n" })
                                      .Select(game => game.Split(' '))
                                      .Select(cards => Tuple.Create(new PokerHand(cards.Take(5).ToArray()), new PokerHand(cards.Skip(5).Take(5).ToArray())))
                                      .ToList();
            int player1Wins = 0;
            int player2Wins = 0;
            
            foreach (var game in result)
            {
                var player1Hand = game.Item1.GetBestHand();
                var player2Hand = game.Item2.GetBestHand();

                if (player1Hand.Item1 > player2Hand.Item1)
                {
                    player1Wins++;
                }

                if (player2Hand.Item1 > player1Hand.Item1)
                {
                    player2Wins++;
                }

                if (player1Hand.Item1 == player2Hand.Item1)
                {
                    if (player1Hand.Item2.HighCard().Value > player2Hand.Item2.HighCard().Value)
                    {
                        player1Wins++;
                    }

                    if (player2Hand.Item2.HighCard().Value > player1Hand.Item2.HighCard().Value)
                    {
                        player2Wins++;
                    }
                }
            }

            if (player1Wins != 376)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* This is a much more difficult version of Problem 18. #
         * It is not possible to try every route to solve this problem, as there are 299 altogether! 
         * If you could check one trillion (1012) routes every second it would take over twenty billion years to check them all. 
         * There is an efficient algorithm to solve it. ;o)*/
        [TestMethod]
        public void Problem67()
        {
            var triangle = FileSequences.Items(@".\Data\Triangle.txt", new string[] { "\r\n" })
                .Select(row => row.Split(' ').Select(items => int.Parse(items)).ToArray())
                .ToArray();

            var maxPath = triangle.CollapseTree();

            if (maxPath != 7273)
            {
                throw new InvalidOperationException("Problem not solved correctly");
            }
        }

        /* Find the last ten digits of the non-Mersenne prime: 28433 × 2^7830457 + 1. */
        [TestMethod]
        public void Problem97()
        {

        }
    }
}
