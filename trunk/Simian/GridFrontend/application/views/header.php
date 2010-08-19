<?php if ( ! isset($this->simple_page) || ! $this->simple_page ):?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>SimianGrid</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<base href="{base_url}" />
<link rel="stylesheet" href="static/style.css" type="text/css" />
<!-- <link rel="icon" href="static/images/icon.ico" /> -->
<script src="static/javascript/jquery.min.js" type="text/javascript" ></script>
<script src="static/javascript/jquery-ui.min.js" type="text/javascript" ></script>
</head>
<body>

<div id="page">

<h1>SimianGrid<span></span></h1>

<menu>
<li><a href="{site_url}/user/search">Users</a></li>
<li><a href="{site_url}/region">Regions</a></li>
<?php if ($this->dx_auth->is_logged_in()): ?>
<li><a href="{site_url}/user">Account</a></li>
<?php endif; ?>
<?php if ($this->dx_auth->is_logged_in() && !strpos(uri_string(), 'logout')): ?>
<li><a href="{site_url}/auth/logout"><span>Log Out</span></a></li>
<?php endif; ?>
<?php if ( ! $this->dx_auth->is_logged_in()): ?>
<li><a href="{site_url}/auth"><span>Login</span></a></li>
<li><a href="{site_url}/auth/register"><span>Join Now</span></a></li>
<?php endif; ?>

<li><a href="{site_url}/about">About</a></li>
</menu>

<div id="border">
<div id="contents">
<?php endif; ?>
