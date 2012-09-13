/*
MBNCSUtil -- Managed Battle.net Authentication Library
Copyright (C) 2005-2008 by Robert Paveza

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met: 

1.) Redistributions of source code must retain the above copyright notice, 
this list of conditions and the following disclaimer. 
2.) Redistributions in binary form must reproduce the above copyright notice, 
this list of conditions and the following disclaimer in the documentation 
and/or other materials provided with the distribution. 
3.) The name of the author may not be used to endorse or promote products derived 
from this software without specific prior written permission. 
	
See LICENSE.TXT that should have accompanied this software for full terms and 
conditions.

*/


using System;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Text;
using BNSharp.MBNCSUtil.Util;
using System.Globalization;
using System.Security.Permissions;
using System.Collections.Generic;

namespace BNSharp.MBNCSUtil
{
    /// <summary>
    /// Encompasses any revision check functionality for all Battle.net games.
    /// This class cannot be inherited.
    /// </summary>
    /// <remarks>
    /// This class provides all CheckRevision-related support, 
    /// including file checksumming and EXE version information.
    /// </remarks>
    /// <threadsafety>This type is safe for multithreaded operations.</threadsafety>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public static class CheckRevision
    {
        /** These are the hashcodes for the various .mpq files. */
        private static readonly uint[] hashcodes =
            new uint[] 
				{ 
					0xE7F4CB62, 
					0xF6A14FFC, 
					0xAA5504AF, 
					0x871FCDC2, 
					0x11BF6A18, 
					0xC57292E6, 
					0x7927D27E, 
					0x2FEC8733 
				};

        /// <summary>
        /// Extracts the MPQ number from the MPQ specified by the Battle.net server.
        /// </summary>
        /// <remarks>
        /// <para></para>
        /// <para>For older CheckRevision calls, the MPQ number is a required parameter of the CheckRevision function.  Note that the MPQ number is simply the number represented
        /// in string format in the 8th position (index 7) of the string -- for example, in "IX86ver<b>1</b>.mpq", 1 is the version number.</para>
        /// </remarks>
        /// <param name="mpqName">The name of the MPQ file specified in the SID_AUTH_INFO message.</param>
        /// <returns>The number from 0 to 7 specifying the number in the MPQ file.</returns>
        /// <exception cref="ArgumentException">Thrown if the name of the MPQ version file is less than 8 characters long.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <i>mpqName</i> parameter is <b>null</b> (<b>Nothing</b> in Visual Basic).
        /// </exception>
        /// <exception cref="NotSupportedException">Thrown if the <i>mpqName</i> parameter indicates a Lockdown DLL.</exception>
        public static int ExtractMPQNumber(string mpqName)
        {
            if (mpqName == null)
                throw new ArgumentNullException("mpqName", Resources.crMpqNameNull);

            if (mpqName.ToUpperInvariant().StartsWith("LOCKDOWN", StringComparison.Ordinal))
                throw new NotSupportedException(Resources.crevExtrMpqNum_NoLockdown);

            if (mpqName.Length < 7)
                throw new ArgumentException(Resources.crMpqNameArgShort);

            string mpqNameUpper = mpqName.ToUpperInvariant();
            int num = -1;

            // ver-IX86-X.mpq
            if (mpqNameUpper.StartsWith("VER", StringComparison.Ordinal))
            {
                num = int.Parse(mpqName[9].ToString(), CultureInfo.InvariantCulture);
            }
            else  // IX86VerX.mpq
            {
                num = int.Parse(mpqName[7].ToString(), CultureInfo.InvariantCulture);
            }

            return num;
        }

        /// <summary>
        /// Calculates the revision check for the specified files.
        /// </summary>
        /// <param name="valueString">The value string for the check revision function specified by Battle.net's SID_AUTH_INFO message.</param>
        /// <param name="files">The list of files for the given game client.  This parameter must be exactly three files long.</param>
        /// <param name="mpqNumber">The number of the MPQ file.  To extract this number, see the 
        /// <see cref="ExtractMPQNumber(String)">ExtractMPQNumber</see> method.</param>
        /// <returns>The checksum value.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <i>valueString</i> or <i>files</i> parameters are <b>null</b>
        /// (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="files" /> is not a 3-string array, or if  
        /// <paramref name="mpqNumber" /> is outside of the range of 0 to 7, inclusive.</exception>
        /// <exception cref="FileNotFoundException">Thrown if one of the specified game files is not found.</exception>
        /// <exception cref="IOException">Thrown in the event of a general I/O error.</exception>
        /// <remarks>
        /// <para>The file list for this is product-specific and order-specific:</para>
        /// <list type="table">
        ///		<listheader>
        ///			<term>Product</term>
        ///			<description>File list</description>
        ///		</listheader>
        ///		<item>
        ///			<term>Starcraft; Starcraft: Brood War</term>
        ///			<description>
        ///				<list type="bullet">
        ///					<item>
        ///						<description>Starcraft.exe</description>
        ///					</item>
        ///					<item>
        ///						<description>storm.dll</description>
        ///					</item>
        ///					<item>
        ///						<description>battle.snp</description>
        ///					</item>
        ///				</list>
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<term>Warcraft II: Battle.net Edition</term>
        ///			<description>
        ///				<list type="bullet">
        ///					<item>
        ///						<description>Warcraft II BNE.exe</description>
        ///					</item>
        ///					<item>
        ///						<description>storm.dll</description>
        ///					</item>
        ///					<item>
        ///						<description>battle.snp</description>
        ///					</item>
        ///				</list>
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<term>Diablo II; Diablo II: Lord of Destruction</term>
        ///			<description>
        ///				<list type="bullet">
        ///					<item>
        ///						<description>Game.exe</description>
        ///					</item>
        ///					<item>
        ///						<description>Bnclient.dll</description>
        ///					</item>
        ///					<item>
        ///						<description>D2Client.dll</description>
        ///					</item>
        ///				</list>
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<term>Warcraft III: The Reign of Chaos; Warcraft III: The Frozen Throne</term>
        ///			<description>
        ///				<list type="bullet">
        ///					<item>
        ///						<description>War3.exe</description>
        ///					</item>
        ///					<item>
        ///						<description>storm.dll</description>
        ///					</item>
        ///					<item>
        ///						<description>Game.dll</description>
        ///					</item>
        ///				</list>
        ///			</description>
        ///		</item>
        /// </list>
        /// </remarks>
        public static int DoCheckRevision(
            string valueString,
            string[] files,
            int mpqNumber)
        {
            if (valueString == null)
                throw new ArgumentNullException("valueString", Resources.crValstringNull);
            if (files == null)
                throw new ArgumentNullException("files", Resources.crFileListNull);
            if (files.Length != 3)
                throw new ArgumentOutOfRangeException("files", files, Resources.crFileListInvalid);
            if (mpqNumber < 0 || mpqNumber > 7)
                throw new ArgumentOutOfRangeException("mpqNumber", mpqNumber, "MPQ number must be between 0 and 7, inclusive.");

            if (AlwaysUseSlowCheck)
            {
                return SlowCheckRevision(valueString, files, mpqNumber);
            }
            else
            {
                switch (OptimizationStrategy)
                {
                    case CheckRevisionOptimizationStrategy.LoadFilesOnDemand:
                        return SlowCheckRevision(valueString, files, mpqNumber);
                    case CheckRevisionOptimizationStrategy.PreloadAndPersistFiles:
                    case CheckRevisionOptimizationStrategy.PreloadAllFilesAndClearAfterUse:
                        return ExecutePreloadedRevisionCheck(valueString, files, mpqNumber);
                    default:
                        throw new InvalidOperationException("Optimization strategy is an unsupported value.");

                } 
            }
        }

        private static int ExecutePreloadedRevisionCheck(
            string valueString,
            string[] files,
            int mpqNumber)
        {
            uint A, B, C;
            List<string> formulas = new List<string>();

            CheckRevisionFormulaTracker.InitializeValues(valueString, formulas, out A, out B, out C);

            A ^= hashcodes[mpqNumber];

            Stream dataStream = CheckRevisionPreloadTracker.GetFiles(files);

            int result = DoCheckRevisionCompiled(A, B, C, formulas, dataStream);

            if (OptimizationStrategy == CheckRevisionOptimizationStrategy.PreloadAllFilesAndClearAfterUse)
            {
                dataStream.Dispose();
                dataStream = null;
            }

            return result;
        }

        // This method always assumes the inputs have been sanity-checked.
        private static int SlowCheckRevision(
            string valueString,
            string[] files,
            int mpqNumber)
        {
            uint[] values = new uint[4];

            int[] opValueDest = new int[4];
            int[] opValueSrc1 = new int[4];
            char[] operation = new char[4];
            int[] opValueSrc2 = new int[4];

            string[] tokens = valueString.Split(new char[] { ' ' });

            int currentFormula = 0;

            // while (stringTokenizer.hasMoreTokens())
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                // as long as there is a '=' in the string
                if (token.IndexOf('=') != -1)
                {
                    string[] nameTokens = token.Split(new char[] { '=' });
                    if (nameTokens.Length != 2)
                        return 0;

                    int variable = getNum(nameTokens[0][0]);
                    string value = nameTokens[1];

                    // If it starts with a number, assign that number to the appropriate variable
                    if (char.IsDigit(value[0]))
                    {
                        values[variable] = uint.Parse(value, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        opValueDest[currentFormula] = variable;

                        opValueSrc1[currentFormula] = getNum(value[0]);
                        operation[currentFormula] = value[1];
                        opValueSrc2[currentFormula] = getNum(value[2]);

                        currentFormula++;
                    }
                }
            }

            // Now we actually do the hashing for each file
            // Start by hashing A by the hashcode
            values[0] ^= hashcodes[mpqNumber];

            byte[] currentOperandBuffer = new byte[1024];
            for (int i = 0; i < files.Length; i++)
            {
                using (FileStream currentFile = new FileStream(files[i], FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    while (currentFile.Position < currentFile.Length)
                    {
                        long currentFilePosition = 0;
                        long amountToRead = Math.Min(currentFile.Length - currentFile.Position, 1024);
                        currentFile.Read(currentOperandBuffer, 0, (int)amountToRead);

                        if (amountToRead < 1024)
                        {
                            byte currentPaddingByte = 0xff;
                            for (int j = (int)amountToRead; j < 1024; j++)
                            {
                                unchecked
                                {
                                    currentOperandBuffer[j] = currentPaddingByte--;
                                }
                            }
                        }
                        CheckRevisionFormulaTracker.FileAppendBytes("c:\\projects\\baseline.bin", currentOperandBuffer);

                        for (int j = 0; j < 1024; j += 4)
                        {
                            values[3] = BitConverter.ToUInt32(currentOperandBuffer, j);

                            for (int k = 0; k < currentFormula; k++)
                            {
                                switch (operation[k])
                                {
                                    case '+':
                                        values[opValueDest[k]] = values[opValueSrc1[k]] + values[opValueSrc2[k]];
                                        break;

                                    case '-':
                                        values[opValueDest[k]] = values[opValueSrc1[k]] - values[opValueSrc2[k]];
                                        break;

                                    case '^':
                                        values[opValueDest[k]] = values[opValueSrc1[k]] ^ values[opValueSrc2[k]];
                                        break;

                                    case '*': // as shady said, you never know.
                                        values[opValueDest[k]] = values[opValueSrc1[k]] * values[opValueSrc2[k]];
                                        break;

                                    case '/': // in case blizz gets "sneaky"
                                        values[opValueDest[k]] = values[opValueSrc1[k]] / values[opValueSrc2[k]];
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return unchecked((int)values[2]);
        }

        // This method assumes all parameters have been sanity checked
        private static int DoCheckRevisionCompiled(
            uint A, 
            uint B, 
            uint C, 
            IEnumerable<string> crevFormulas,
            Stream completeDataStream)
        {
            StandardCheckRevisionImplementation impl = CheckRevisionFormulaTracker.GetImplementation(crevFormulas);

            using (BinaryReader br = new BinaryReader(completeDataStream))
            {
                uint S;
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    S = br.ReadUInt32();
                    impl(ref A, ref B, ref C, ref S);
                }
            }
            return unchecked((int)C);
        }

        /// <summary>
        /// Performs the Lockdown revision check.
        /// </summary>
        /// <param name="valueString">The value string parameter, not including the null terminator.</param>
        /// <param name="gameFiles">The three game files.  This parameter must be exactly three files long.</param>
        /// <param name="lockdownFile">The path to the lockdown file requested.</param>
        /// <param name="imageFile">The path to the screen dump.</param>
        /// <param name="version">[return value] The EXE version.</param>
        /// <param name="checksum">[return value] The EXE hash.</param>
        /// <returns>The "EXE Information" data.  This value should be null-terminated when being inserted into the authorization packet.</returns>
        /// <remarks>
        /// <para>The file list for this is product-specific and order-specific:</para>
        /// <list type="table">
        ///		<listheader>
        ///			<term>Product</term>
        ///			<description>File list</description>
        ///		</listheader>
        ///		<item>
        ///			<term>Starcraft; Starcraft: Brood War</term>
        ///			<description>
        ///				<list type="bullet">
        ///					<item>
        ///						<description>Starcraft.exe</description>
        ///					</item>
        ///					<item>
        ///						<description>storm.dll</description>
        ///					</item>
        ///					<item>
        ///						<description>battle.snp</description>
        ///					</item>
        ///				</list>
        ///			</description>
        ///		</item>
        ///		<item>
        ///			<term>Warcraft II: Battle.net Edition</term>
        ///			<description>
        ///				<list type="bullet">
        ///					<item>
        ///						<description>Warcraft II BNE.exe</description>
        ///					</item>
        ///					<item>
        ///						<description>storm.dll</description>
        ///					</item>
        ///					<item>
        ///						<description>battle.snp</description>
        ///					</item>
        ///				</list>
        ///			</description>
        ///		</item>
        /// </list>
        /// </remarks>
        public static byte[] DoLockdownCheckRevision(
            byte[] valueString,
            string[] gameFiles,
            string lockdownFile,
            string imageFile,
            ref int version,
            ref int checksum)
        {
            byte[] digest;
            LockdownCrev.CheckRevision(gameFiles[0], gameFiles[1], gameFiles[2], valueString, ref version, ref checksum, out digest, lockdownFile, imageFile);

            return digest;
        }

        private static int getNum(char c)
        {
            c = char.ToUpper(c, CultureInfo.InvariantCulture);
            if (c == 'S')
                return 3;
            else
                return c - 'A';
        }

        /// <summary>
        /// Gets EXE information for the specified file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="exeInfoString">Returns the file's timestamp and other information.</param>
        /// <returns>The file's version.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <i>fileName</i> parameter is <b>null</b> (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="FileNotFoundException">Thrown if the file specified by <i>fileName</i> does not exist in the specified path.</exception>
        public static int GetExeInfo(
            string fileName,
            out string exeInfoString)
        {
            if (fileName == null)
                throw new ArgumentNullException(Resources.param_fileName, Resources.crExeFileNull);

            string file = fileName.Substring(fileName.LastIndexOf('\\') + 1);
            uint fileSize = 0;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileSize = unchecked((uint)fs.Length);
            }

            DateTime ft = File.GetLastWriteTimeUtc(fileName);

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fileName);
            /* // updated version
			int version = ((fvi.FileMajorPart & 0x00ff0000) << 8) |
				((fvi.FileMajorPart & 0x000000ff) << 16) |
				((fvi.FileMinorPart & 0x00ff0000) >> 8) |
				(fvi.FileMinorPart & 0x000000ff);
            */
            int version = ((fvi.FileMajorPart << 24) |
                (fvi.FileMinorPart << 16) |
                (fvi.FileBuildPart << 8) | fvi.FilePrivatePart);

            exeInfoString = String.Format(CultureInfo.InvariantCulture, 
                Resources.exeInfoFmt,
                file, ft.Month, ft.Day, ft.Year % 100, ft.Hour, ft.Minute, ft.Second, fileSize
                );

            return version;
        }

        /// <summary>
        /// Gets the current "version byte" for the specified product.
        /// </summary>
        /// <remarks>
        /// <para>Only the following product IDs can be used with the web service: STAR, SEXP, W2BN, D2DV, D2XP,
        /// WAR3, and W3XP.  Other product IDs will result an a <see cref="NotSupportedException">NotSupportedException</see>
        /// being thrown.</para>
        /// <para><span style="color: red;">This method is new and currently in testing.  W2BN, D2DV, and D2XP are currently
        /// unsupported.</span></para>
        /// </remarks>
        /// <param name="productID">The four-character product ID for the product in question.</param>
        /// <exception cref="NotSupportedException">Thrown in all cases.</exception>
        /// <returns>The version byte of the product.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "productID")]
        [Obsolete("Use of the MBNCSUtil web service for retrieving version bytes is no longer possible.", true)]
        public static byte GetVersionByte(string productID)
        {
            throw new NotSupportedException();
            /*
            string val = productID.ToUpper();
            WebRequest req = HttpWebRequest.Create("http://www.jinxbot.net/mbncsutil/mbncsutil_bytes.aspx?Client=" + val);
            WebResponse resp = req.GetResponse();
            Stream strm = resp.GetResponseStream();
            StreamReader sr = new StreamReader(strm);
            int ver = int.Parse(sr.ReadLine());
            return (byte)ver;
             * */
        }

        /// <summary>
        /// Gets or sets whether to use the slow revision check.
        /// </summary>
        /// <remarks>
        /// <para>By default, the revision check operation for non-Lockdown clients uses dynamic compilation based on 
        /// the standard format of the revision check formula (<c>A=x B=y C=z 4 A=A?S B=B?C C=C?A A=A?B</c>).  If there
        /// is a compatibility problem, clients can disable the use of dynamic compilation by setting this property
        /// to <see langword="true" />.</para>
        /// <para>In the current version of MBNCSUtil and BN#, setting this property to <see langword="true" /> has the effect of causing the
        /// revision check process to use the <see cref="CheckRevisionOptimizationStrategy.LoadFilesOnDemand">LoadFilesOnDemand</see> 
        /// <see cref="OptimizationStrategy">optimization strategy</see>, regardless of the value of that property, to ensure 100% compatibility with
        /// the older version of the revision check formulas.  Future versions will vary according to that property.</para>
        /// </remarks>
        [DefaultValue(false)]
        public static bool AlwaysUseSlowCheck
        {
            get;
            set;
        }

        private static CheckRevisionOptimizationStrategy _optimizationStrategy = CheckRevisionOptimizationStrategy.PreloadAllFilesAndClearAfterUse;
        /// <summary>
        /// Gets or sets the optimization strategy used by the revision check file loader.
        /// </summary>
        /// <remarks>
        /// <para>For more information about the differences between the optimization strategies, see 
        /// <see>CheckRevisionOptimizationStrategy</see>.</para>
        /// <para>By default, this is set to <see>CheckRevisionOptimizationStrategy.PreloadAllFilesAndClearAfterUse</see>.
        /// </para>
        /// </remarks>
        [DefaultValue(CheckRevisionOptimizationStrategy.PreloadAllFilesAndClearAfterUse)]
        public static CheckRevisionOptimizationStrategy OptimizationStrategy
        {
            get { return _optimizationStrategy; }
            set
            {
                if (!Enum.IsDefined(typeof(CheckRevisionOptimizationStrategy), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(CheckRevisionOptimizationStrategy));
                _optimizationStrategy = value;
            }
        }
    }

    /// <summary>
    /// Specifies the optimization strategy that <see>CheckRevision</see> should use when loading files from disk.
    /// </summary>
    public enum CheckRevisionOptimizationStrategy
    {
        /// <summary>
        /// Reads all the files and inserts the appropriate padding before executing the revision check, allowing the 
        /// operation to execute in a single pass without (generally) needing to hit the disk.  This is the default 
        /// optimization strategy, and will use about 11mb of memory transitionally during the operation of the check,
        /// but will free it following the check.
        /// </summary>
        PreloadAllFilesAndClearAfterUse,
        /// <summary>
        /// Reads all of the files and inserts the appropriate padding before executing the revision check, based on a 
        /// combination of the names of the files that are being used.  This allows the operation to execute in a single
        /// pass without needing to hit the disk.  When the revision check is completed, the file data are retained so 
        /// that future calls don't need to recombine the file data.  For clients that use classic revision check, this
        /// means that files will take about 11mb each.  For clients that use lockdown, because the lockdown file is part
        /// of the hashed file data, this may be up to about 200mb.  This strategy is probably ideal for applications 
        /// similar to BNLS that need to focus on serving revision check requests, and not for general client use.
        /// </summary>
        PreloadAndPersistFiles,
        /// <summary>
        /// Reads files one kilobyte at a time, as the revision check needs the file data to be read in.  This is probably 
        /// the slowest optimization strategy, but it probably will hit the disk more than once during the operation.
        /// It also uses the least amount of memory.
        /// </summary>
        LoadFilesOnDemand
    }
}

