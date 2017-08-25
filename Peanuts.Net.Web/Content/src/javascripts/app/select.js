import 'select2/dist/js/select2.full';
import 'select2/dist/js/i18n/de';

const Select = (function ($) {
    const selectors = '.js-select-single,.js-select-multiple';
    const selects = $(selectors);
    const options = {
        language: "de",
        width: null
    };

    if( selects.length ) {
        selects.each(function () {
            let _options = $.extend({}, options);
            if( $(this).find('option').length < 10 ) {
                _options['minimumResultsForSearch'] = -1;
            }
            $(this)
                .on('change', function(){
                    $(this).valid()
                })
                .select2(_options);
        });
    }

}(jQuery));

const SelectDepending = (function ($) {
    
    const dependingSelectsSelector = 'select[js-depends-on]';

    function initDependingSelect($select) {

        function filterOptions(byValue) {
            /*aktuell ausgew�hlten Wert merken*/
            var selectedValue = $select.val();

            /*alle Optionen aus dem Select entfernen*/
            $select.find('option').remove().end();

            /*Nur die passenden Optionen wieder in das Select eintragen*/
            $select.append(options.filter(function() {
                if (byValue) {
                    /*Es ist ein Wert im Parent-Select ausgew�hlt*/
                    if ($(this).attr("js-depends-on-value") == byValue) {
                        /*Diese Option ist f�r den im Parent ausgew�hlten Wert g�ltig!*/
                        if (!($(this).val())) {
                            /*Platzhalter austauschen, wenn die Option keinen Value hat*/
                            $select.data("placeholder", $(this).text())
                            $select.attr("placeholder", $(this).text())
                        }

                        return true;
                    }
                } else {
                    /*Es ist ein Wert im Parent-Select ausgew�hlt*/
                    if (!this.value || $.trim(this.value).length == 0) {
                        /*Platzhalter bleibt drin*/
                        return true;
                    }

                    if (($(this).attr("js-depends-on-value"))) {
                        /*Der Wert ist abh�ngig von einer Auswahl im Parent und darf nicht angezeigt werden.*/
                        return false;
                    } else {
                        /*Der Wert ist unabh�ngig von einer Auswahl.*/
                        return true;
                    }
                        
                }
                    
                    
            }));

            /*alten Wert wieder ausw�hlen bzw. Platzhalter-Wert ausw�hlen.*/
            $select.val(selectedValue);

            /*Select2 aktualisieren*/
            $select.select2();
        }

        var $dependsOn = $('#' + $select.attr('js-depends-on'));
        var options = $select.find("option");

        if ($dependsOn) {

            /*initiale Anzeige*/
            filterOptions($dependsOn.val());

            /*auf �nderungen reagieren*/
            $dependsOn.on('change', function(){
                /*Das Select, von welchem dieses Select abh�ngig ist, wurde ge�ndert.*/
                filterOptions($dependsOn.val());
            });
        }
    }

    function findAndInitSelects($parent) {
        var selects = $(dependingSelectsSelector, $parent);
        if( selects.length ) {
            selects.each(function (index, select) {
                initDependingSelect($(select))
            });
        }
    }

    /* Alle initial vorhandenen Selects initialisieren */
    findAndInitSelects($(document));

    /*Dynamisch hinzugef�gte Elemente initialisieren*/
    $(".dynamic-list").on("dynamic-list:itemAdded", function(e) {
        var $listItem = $(e.item);
        findAndInitSelects($listItem);
    });

}(jQuery));

export default Select;