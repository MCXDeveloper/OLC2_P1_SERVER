Blockly.Blocks['sentencia_use'] = {
    init: function() {
        this.appendDummyInput().appendField(new Blockly.FieldLabelSerializable("Use"), "_use_").appendField(new Blockly.FieldTextInput("Database"), "nombre_db");
        this.setPreviousStatement(true, null);
        this.setNextStatement(true, null);
        this.setColour(330);
        this.setTooltip("");
        this.setHelpUrl("");
    }
};

Blockly.JavaScript['sentencia_use'] = function(block) {
    var text_nombre_db = block.getFieldValue('nombre_db');
    // TODO: Assemble JavaScript into code variable.
    var code = 'USE ' + text_nombre_db + ';\n';
    return code;
};