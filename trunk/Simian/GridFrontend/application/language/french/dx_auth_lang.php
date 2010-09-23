<?php

/*
It is recommended for you to change 'auth_login_incorrect_password' and 'auth_login_username_not_exist' into something vague. 
For example: Username and password do not match.
*/

$lang['auth_login_incorrect_password'] = "Votre mot de pass est incorrecte.";
$lang['auth_login_username_not_exist'] = "Le nom d'utilisateur n'existe pas.";

$lang['auth_username_or_email_not_exist'] = "Le nom d'utilisateur ou l'Email n'existe pas.";
$lang['auth_not_activated'] = "Votre compte n'a pas encore �t� activ�. S'il vous pla�t consulter votre Email.";
$lang['auth_request_sent'] = "Votre demande de changement de mot de passe a d�j� �t� envoy�e. S'il vous pla�t consulter votre Email.";
$lang['auth_incorrect_old_password'] = "Votre ancien mot de passe est incorrect.";
$lang['auth_incorrect_password'] = "Votre mot de passe est incorrect.";

// Email subject
$lang['auth_account_subject'] = "%s d�tails de votre compte.";
$lang['auth_activate_subject'] = "%s activation.";
$lang['auth_forgot_password_subject'] = "Demander un nouveau mot de pass.";

// Email content
$lang['auth_account_content'] = "Bienvenue sur %s,

Merci pour votre inscription. Votre compte a �t� cr�� avec succ�s.

Vous pouvez vous connecter soit avec votre nom d'utilisateur ou votre adresse e-mail:

Identifiant: %s
Adresse Email: %s
Mot de Pass: %s

Vous pouvez essayez de vous connecter maintenant en allant sur %s

Nous esp�rons que vous appr�cierez votre s�jour chez nous.

Cordialement,
The %s Team";

$lang['auth_activate_content'] = "Bienvenue sur %s,

Pour activer votre compte, vous devez suivre le lien d'activation ci-dessous:
%s

S'il vous plait activer votre compte en %s heures, faute de quoi votre inscription ne sera pas valide et vous devrez vous inscrire � nouveau.

Vous pouvez utiliser votre nom d'utilisateur ou adresse e-mail pour vous connecter.
Vos informations de connexion sont les suivantes:

Identifiant: %s
Adresse Email: %s
Mot de Pass: %s

Nous esp�rons que vous appr�cierez votre s�jour chez nous :)

Cordialement,
The %s Team";

$lang['auth_forgot_password_content'] = "%s,

Vous avez demand� votre mot de passe doit �tre chang�, parce que vous avez oubli� le mot de passe.
S'il vous pla�t suivez ce lien afin de terminer le processus changer mot de passe:
%s

Votre nouveau mot de passe: %s
Cl�s d'activation: %s

Apr�s avoir r�ussi � achever le processus, vous pouvez modifier ce nouveau mot de passe dans le mot de passe que vous voulez.

Si vous avez des probl�mes avec plus d'acc�der � votre compte s'il vous pla�t contacter %s.

Cordialement,
The %s Team";

?>
