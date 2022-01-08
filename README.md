# Cryptographic-Hashing-for-Bitcoing-Mining-F-Actor-Model-AKKA

Introduction :
Bitcoins are the most popular crypto-currency in common use. At their heart, bitcoins use the hardness of cryptographic hashing to ensure a limited “supply” of coins.  In particular, the key component in a bit-coin is an input that, when “hashed” produces an output smaller than a target value.  In practice, the comparison values have leading  0’s, thus the bitcoin is required to have a given number of leading 0’s (to ensure 3 leading 0’s, you look for hashes smaller than0x001000... or smaller or equal to 0x000ff....). The hash you are required to use is SHA-256. The goal of this first project is to use F# and the actor model to build a good solution to this problem that runs well on multi-core machines.

Requirements :
Input: The input provided (as command line to yourproject1.fsx) will be, the required number of 0’s of the bitcoin.
Output: Print, on independent : entry lines, the input string, and the correspondingSHA256 hash separated by a TAB, for each of the bitcoins you find. Obviously, your SHA256 hash must have the required number of leading 0s (k= 3 means3 0’s in the hash notation).  An extra requirement, to ensure every group finds different coins, is to have the input string prefixed by the gator link ID of one of the team members.

Example 1:
1
adobra;kjsdfk11 0d402337f95d018438aad6c7dd75ad6e9239d6060444a7a6b26299b261aa9a8b
The above example indicates that the coin with 1 leading 0 is adobra;kjsdfk11and it is prefixed by the gatorlink ID adobra.

Actor modeling :
In this project, you have to use exclusively the AKKA actor library in F# (projects that do not use multiple actors or use any other form of parallelism will receive no credit).  In particular, define worker actors that are given a range of problems to solve and boss that keeps track of all the problems and perform the job assignment.
