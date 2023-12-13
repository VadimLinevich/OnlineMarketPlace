const sortButtons = document.querySelectorAll(".sort-button");

for(let i = 0; i < sortButtons.length; i++)
{
    sortButtons[i].addEventListener("click", () => {
    if (sortButtons[i].classList.contains("sort-button")) {
        sortButtons[i].classList.replace("sort-button", "sort-button-active")
        for(let j = 0; j < sortButtons.length; j++)
        {
          if(j != i) {
            sortButtons[j].classList.replace("sort-button-active", "sort-button")
          }
        }
        // sortButtons[i].classList.remove("sort-button");
        // sortButtons[i].classList.add("sort-button-active");
    }
  });
}