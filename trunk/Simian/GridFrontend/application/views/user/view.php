<h2>User View</h2>
    
<div id="user_menu">
     <ul>
         <li><?php echo anchor('user/profile/' . $this->uuid, 'Profile'); ?></li>
<? if ( $this->dx_auth->is_logged_in() ): ?>
    <? if ( $this->uuid == $this->my_uuid || $this->dx_auth->is_admin() ): ?>
         <li><?php echo anchor('user/password/' . $this->uuid, 'Password'); ?></li>
         <li><?php echo anchor('user/identities/' . $this->uuid, 'Identities'); ?></li>
    <? endif; ?>
    <? if ( $this->dx_auth->is_admin() ): ?>
        <li><?php echo anchor('user/actions/' . $this->uuid, 'Actions'); ?></li>
        <li><?php echo anchor('user/raw/' . $this->uuid, 'Raw'); ?></li>
    <? endif ; ?>
<? endif ; ?>
     </ul>
</div>

<script>
    $( "#user_menu" ).tabs({ ajaxOptions: { async: false } });
</script>