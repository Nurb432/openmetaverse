<?php
$first_name = array(
	'name'	=> 'first_name',
	'id'	=> 'first_name',
	'size'	=> 30,
	'value' =>  set_value('first_name')
);

$last_name = array(
	'name'	=> 'last_name',
	'id'	=> 'last_name',
	'size'	=> 30,
	'value' =>  set_value('last_name')
);

$password = array(
	'name'	=> 'password',
	'id'	=> 'password',
	'size'	=> 30,
	'value' => set_value('password')
);

$confirm_password = array(
	'name'	=> 'confirm_password',
	'id'	=> 'confirm_password',
	'size'	=> 30,
	'value' => set_value('confirm_password')
);

$email = array(
	'name'	=> 'email',
	'id'	=> 'email',
	'maxlength'	=> 80,
	'size'	=> 30,
	'value'	=> set_value('email')
);

$captcha = array(
	'name'	=> 'captcha',
	'id'	=> 'captcha'
);
?>

<fieldset><legend>Register</legend>
<?php echo form_open($this->uri->uri_string())?>

<?php echo $this->dx_auth->get_auth_error(); ?>

<dl>
	<dt><?php echo form_label('First Name', $first_name['id']);?></dt>
	<dd>
		<?php echo form_input($first_name)?>
    <?php echo form_error($first_name['name']); ?>
	</dd>
	
	<dt><?php echo form_label('Last Name', $last_name['id']);?></dt>
	<dd>
		<?php echo form_input($last_name)?>
    <?php echo form_error($last_name['name']); ?>
	</dd>

	<dt><?php echo form_label('Password', $password['id']);?></dt>
	<dd>
		<?php echo form_password($password)?>
    <?php echo form_error($password['name']); ?>
	</dd>

	<dt><?php echo form_label('Confirm Password', $confirm_password['id']);?></dt>
	<dd>
		<?php echo form_password($confirm_password);?>
		<?php echo form_error($confirm_password['name']); ?>
	</dd>

	<dt><?php echo form_label('Email Address', $email['id']);?></dt>
	<dd>
		<?php echo form_input($email);?>
		<?php echo form_error($email['name']); ?>
	</dd>
		
<?php if ($this->dx_auth->captcha_registration): ?>

	<dt>Enter the code exactly as it appears. There is no zero.</dt>
	<dd><?php echo $this->dx_auth->get_captcha_image(); ?></dd>

	<dt><?php echo form_label('Confirmation Code', $captcha['id']);?></dt>
	<dd>
		<?php echo form_input($captcha);?>
		<?php echo form_error($captcha['name']); ?>
	</dd>
	
<?php endif; ?>

	<dt></dt>
	<dd><?php echo form_submit('register','Register', 'class="button"');?></dd>
</dl>

<?php echo form_close()?>
</fieldset>

