# **Introduction** #

For now I'll cover briefly what I installed, in no particular order excepting the base ubuntu, which necesarily came first.

Later, as I have more detail, I'll make some notes concerning windows environments.

# **Windows Environment Details** #

I have been able to run the installer successfully in a Windows 2008 Web Edition environment running Apache 2.2.16 from apache.org, php 5.3 for windows from PHP.net, and MySQL 5.1 from oracle.com as of 2010 09 27


# **Linux Environment Details** #

This may or may not be the preferred/ideal development environment, but it's the one that I have found best satisfies the dependencies.

After a base installation of Ubuntu Lucid, I installed the repositories package, updated package lists, and upgraded everything.

I then installed:<br>
apache 2.2.14<br>
php5.3.2<br>
MariaDB 5.1<br>
subversion<br>
openssh<br>
git<br>

I also installed php-cli, but its probably not necesary to the success of the environment. I should probably updgrade everything again, or else install all the forgoing before upgrading.<br>
<br>
Also, per the <a href='Installation.md'>Installation</a> instructions, I installed the PHP extensions 'php5-mysql php5-curl php5-gd' and 'php5-xmlrpc'.<br>
<br>
Note that in addition to the forgoing, in order to satisfy an unarticulated need for Imagemagick, you will need to install 'php-pear php5-dev' and 'libmagickwand-dev', and then run 'pecl install imagick'. Don't miss the instructions in the last few lines of output from that last command pertaining to adding the imagick extension to php.ini. After that's been done, restart the apache server.<br>
<br>
If you are on windows, my best advice is to read this thread:<br>
<br>
<a href='http://www.php.net/manual/en/imagick.installation.php'>http://www.php.net/manual/en/imagick.installation.php</a>

pay very close attention to spellings and read it all before you try to follow any instructions, some of the posts are tales of woe.<br>
<br>
<h1><b>Concerning Git and GitHub workflows</b></h1>

<b>this section assumes a certain level of familiarity with git source code revision control software</b>

Git is complex and powerful software for managing parallel software development timelines.  while it has something of a steep learning curve, getting a basic working knowledge of git is well worth the effort.<br>
<br>
Note too that I am a relative novice with git, and that some of these steps may be redundant or unnecessary. If you find a way of improving this workflow, please edit this document to reflect it.<br>
<br>
Here is how I currently employ git in conjunction with github in the pursuit of working on SimianGrid code:<br>
<br>
1. If you dont already have an account at github.com, get one. Make sure that you establish ssh keys for the account; this is done with some variation of ssh-keygen on your local box, and the public key posted up on the appropriate page at github.com. Then find the OpenMetaverseFoundation project and locate SimianGrid.<br>
<br>
2. Fork SimianGrid using the fork button. This will create your own personal hosted fork of the SimianGrid project on github.<br>
<br>
3. Login to your development server's shell. Clone your personal github fork of SimianGrid using the github-provided URL. You'll want to use the <b>private</b> url as you will want write access.<br>
<br>
4. change directories to the base of your clone and execute the following git commands (from the github website help):<br>
<br>
git remote add upstream git://github.com/openmetaversefoundation/simiangrid.git<br>
<br>
git fetch upstream<br>
<br>
This sets the OpenMetaverse Foundation's SimianGrid project as the upstream source of your fork. In this workflow, you will manage the master copy of your work locally, and push copies to your github fork to keep your published fork completely up-to-date.<br>
<br>
5. You are now ready to get to work. The very first thing to do is create a branch to work in. Note that this is shell work, all taking place on your development machine. Creating a new branch is easy with the command 'git branch working', which creates a new working branch. It could have been named just about anything other than an existing branch name. You can see existing branch names by simply issuing the command 'git branch'. Neither of these commands <b>change</b> branches though. To do that, you would say 'git checkout working', in the context of this discussion.<br>
<br>
6. Let us say you have made an important bugfix to Grid/hypergrid.php, having first changed to your working branch. You are now ready to commit it to your local repository. This is done by issuing the command 'git commit -a'. Note that this form of the commit command to git will commit all tracked files which have changed. Git will tell you if you have untracked files which have changed. Committing with git is a complex topic that we wont explore here further; suffice it to say, this will commit a changed file already in the repo.<br>
<br>
7. Next, you will want to update your local repo's master branch, so that your fixes will be merged to the latest code. This prevents conflicts. This is accomplished by changing back to the master branch, and issuing a pull from upstream, as in 'git pull upstream master'. Once this completes, you will have the latest code in your master branch, and some older code with your changes in your working branch. Time for the merge :)<br>
<br>
8. To merge your changes, you will need the commit UUID for your commit. The way I get it is to examine the git log immediately after my commit, and copy it to the clipboard for future use. One may also switch branches and do this later to get a given commit UUID. Merge happens with the command 'git merge COMMITUUID', where COMMITUUID is the UUID from your commit. If all has been properly done, your merge should be successful, and you now have a local copy of the latest code, incorporating your changes, in your master branch of your local repo. Now to publish to github.<br>
<br>
9. Publishing to github is simple, all you do is issue the command 'git push'. Having completed the push, you are now ready to issue a pull request to upstream (whoever is managing the repo at Open Metaverse Foundation). If you use github to generate the pull request, it really wont matter <b>who</b> is managing the repo.<br>
<br>
<br>
<br>
<i>Like most such pages, this one is a work in progress - I'll update/edit it as I have information in greater detail.</i>