const getArticle = async() => {
    const searchParams = new URLSearchParams(location.search);
    const id = searchParams.get('id');

    const response = await fetch(`https://<your-app-name>.herokuapp.com/v1/posts/${id}`);
    const data = await response.json();

    document.title = data.title;
    document.querySelector('#title').textContent = data.title;
    if (data.cover_image) {
        document.querySelector('#cover').src = data.cover_image;
    } else {
        document.querySelector('#cover').style.display = 'none';
    }

    document.querySelectorAll('.name').forEach(el => el.textContent = data.author.name);

    var date = new Date(data.publishedDate).toDateString().substring(3)

    document.querySelector('#date').textContent = date;

    const tagList = document.querySelector('#tags');

    for (const tag of data.tags) {
        const li = document.createElement('li');
        li.textContent = tag.name;
        tagList.appendChild(li);
    }

    document.querySelector('#article-body').innerHTML = data.content.replace(/\/assets\//g, './assets/');
    document.querySelectorAll('blockquote').forEach(e => {
        e.addEventListener('click', ({ target }) => {
            if (!target.href && !target.parentElement.href) {
                window.open(e.dataset.url, '_blank');
            }
        })
    });
}

getArticle();