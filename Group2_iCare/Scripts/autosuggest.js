<script type="text/javascript">
    $(document).ready(function() {
        $("#descript").on("input", function() {
            let query = $(this).val().split(" ").pop();

            if (query.length >= 2) {
                $.ajax({
                    url: '@Url.Action("GetDrugSuggestions", "Drug")',
                    data: { query: query },
                    dataType: 'json',
                    success: function(data) {
                        displaySuggestions(data);
                    }
                });
            } else {
                clearSuggestions();
            }
        });
    });

    function displaySuggestions(suggestions) {
        clearSuggestions();
        $.each(suggestions, function(index, suggestion) {
            let suggestionItem = $("<p>")
                .addClass("suggestion-item")
                .html("<strong>" + suggestion.DrugName + "</strong>: " + suggestion.DrugDescription);
            
            suggestionItem.click(function() {
                addSuggestionToDescription(suggestion.DrugName);
                clearSuggestions();
            });
            
            $("#suggestions").append(suggestionItem);
        });
    }

    function addSuggestionToDescription(suggestion) {
        let currentText = $("#descript").val();
        let lastIndex = currentText.lastIndexOf(" ");
        let newText = currentText.substring(0, lastIndex + 1) + suggestion + " ";
        $("#descript").val(newText);
    }

    function clearSuggestions() {
        $("#suggestions").empty();
    }
</script>