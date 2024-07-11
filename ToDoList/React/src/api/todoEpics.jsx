import { ofType } from 'redux-observable';
import { mergeMap} from 'rxjs/operators';
import { from, of } from 'rxjs';
import { updateId } from '../redux/todoSlice';
import { getOptions } from './options';

export const sendMutation = (query, currentDatabase) => {
    return fetch(getOptions().apiEndPoint, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'CurrentDatabase': currentDatabase
      },
      body: JSON.stringify({
        query: query
      })
    })
};

export const addTodoEpic = (action$, state$) =>
  action$.pipe(
    ofType('todos/addTodo'),
    mergeMap(action =>
      from(sendMutation(
        ` mutation {
                notes_M {
                    add(name: "${action.payload.name}", 
                        description: "${action.payload.description}",
                        deadline: ${action.payload.deadline ? `"${action.payload.deadline}:00Z"` : 'null'},
                        categoryIds: ${JSON.stringify(action.payload.categoriesNotes)}) 
                    {
                        id
                    }
                }
            }
        `, state$.value.database
    ))
    .pipe(
        mergeMap(response => response.json()),
        mergeMap((data) => {
            return of(updateId({
                oldId: action.payload.id,
                newId: data.data.notes_M.add.id
            }));
        })
      )
    )
);

export const finishTodoEpic = (action$, state$) =>
    action$.pipe(
      ofType('todos/finishTodo'),
      mergeMap(action =>
        from(sendMutation(
          ` mutation {
                  notes_M {
                      update(id: ${action.payload.id}) 
                      {
                          id
                      }
                  }
              }
          `, state$.value.database
      )).pipe(
          mergeMap(() => {
            return of();
          })
        )
      )
);

export const deleteTodoEpic = (action$, state$) =>
    action$.pipe(
      ofType('todos/deleteTodo'),
      mergeMap(action =>
        from(sendMutation(
          ` mutation {
                  notes_M {
                      delete(id: ${action.payload.id}){
                        name
                      }
                  }
              }
          `, state$.value.database
      )).pipe(
          mergeMap(() => {
              return of();
          })
        )
      )
);