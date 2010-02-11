import os
import sys

r = 1
while True:
	cmd = 'java -jar ../../Antlr/antlr.jar Calculator.g'
	r = os.system(cmd)
	print("Done")
	sys.stdin.readline()
