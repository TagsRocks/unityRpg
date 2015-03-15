import os
f = os.listdir('.')
for i in f:
	if i.find('@') != -1:
		os.system('svn add %s@' % (i))
